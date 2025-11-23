using IELTS.DAL;
using IELTS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class TestSectionBLL
    {
        private readonly TestSectionDAL _sectionDAL;
        private readonly QuestionDAL _questionDAL;
        private readonly QuestionAnswerKeyDAL _answerKeyDAL;

        public TestSectionBLL()
        {
            _sectionDAL = new TestSectionDAL();
            _questionDAL = new QuestionDAL();
            _answerKeyDAL = new QuestionAnswerKeyDAL();
        }

        /// <summary>
        /// Lưu toàn bộ TestSection với Questions và Answers
        /// </summary>
        public bool SaveTestSection(long paperId, string skill, int? timeLimitMinutes,
        string pdfFileName, string pdfFilePath,
        Dictionary<int, string> questionTypes, Dictionary<int, string> answers)
        {
            try
            {
                // 1. Tạo TestSection
                TestSectionDTO section = new TestSectionDTO
                {
                    PaperId = paperId,
                    Skill = skill,
                    TimeLimitMinutes = timeLimitMinutes,
                    PdfFileName = pdfFileName,
                    PdfFilePath = pdfFilePath
                };

                long sectionId = _sectionDAL.InsertTestSection(section);
                if (sectionId <= 0)
                    return false;

                // 2. Tìm các đoạn liên tiếp (start - end)
                var ranges = FindContinuousRanges(questionTypes);

                // 3. Tạo map vị trí -> (start, end) để tra nhanh
                Dictionary<int, (int start, int end)> rangeMap = new Dictionary<int, (int, int)>();
                foreach (var r in ranges)
                {
                    for (int i = r.start; i <= r.end; i++)
                    {
                        rangeMap[i] = (r.start, r.end);
                    }
                }

                // 4. Lưu từng Question + Answer
                foreach (var kvp in questionTypes)
                {
                    int position = kvp.Key;
                    string questionType = kvp.Value;

                    if (string.IsNullOrEmpty(questionType))
                        continue;

                    string answer = answers.ContainsKey(position) ? answers[position] : "";

                    // === ✔ logic xác định QuestionText + Points ===
                    int start = position;
                    int end = position;
                    decimal points = 1;
                    string text = $"Question {position}";

                    if (rangeMap.ContainsKey(position))
                    {
                        start = rangeMap[position].start;
                        end = rangeMap[position].end;

                        text = $"Question {start} - {end}";
                        points = end - start + 1;
                    }

                    // Tạo Question
                    QuestionDTO question = new QuestionDTO
                    {
                        SectionId = sectionId,
                        QuestionType = questionType,
                        QuestionText = text,
                        Points = points,
                        Position = position
                    };

                    long questionId = _questionDAL.InsertQuestion(question);
                    if (questionId <= 0)
                        continue;

                    // Save answer key
                    if (!string.IsNullOrEmpty(answer))
                    {
                        QuestionAnswerKeyDTO answerKey = new QuestionAnswerKeyDTO
                        {
                            QuestionId = questionId,
                            AnswerData = ConvertAnswerToJson(questionType, answer)
                        };

                        _answerKeyDAL.InsertAnswerKey(answerKey);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving test section: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private List<(int start, int end, string type)> FindContinuousRanges(Dictionary<int, string> questionTypes)
        {
            List<(int start, int end, string type)> ranges = new List<(int start, int end, string type)>();

            string[] groupTypes = { "MULTI_CHOICES", "MATCHING", "ORDERING" };

            int start = -1;
            string currentType = null;

            for (int i = 1; i <= 40; i++)
            {
                string type = questionTypes.ContainsKey(i) ? questionTypes[i] : null;

                if (type != null && groupTypes.Contains(type))
                {
                    if (currentType == null)
                    {
                        currentType = type;
                        start = i;
                    }
                    else if (currentType != type)
                    {
                        ranges.Add((start, i - 1, currentType));

                        currentType = type;
                        start = i;
                    }
                }
                else
                {
                    if (currentType != null)
                    {
                        ranges.Add((start, i - 1, currentType));
                        currentType = null;
                    }
                }
            }

            if (currentType != null)
                ranges.Add((start, 40, currentType));

            return ranges;
        }

        /// <summary>
        /// Chuyển đổi answer string thành JSON format
        /// </summary>
        private string ConvertAnswerToJson(string questionType, string answer)
        {
            switch (questionType)
            {
                case "MCQ":
                    // Format: "A"
                    return JsonConvert.SerializeObject(new { answer = answer });

                case "MULTI_SELECT":
                    // Format: "A,C,E"
                    var multiAnswers = answer.Split(',');
                    return JsonConvert.SerializeObject(new { answers = multiAnswers });

                case "FILL_BLANK":
                case "SHORT_ANSWER":
                    // Format: "text"
                    return JsonConvert.SerializeObject(new { answer = answer });

                case "MATCHING":
                    // Format: "Item1:A;Item2:B"
                    var matchDict = new Dictionary<string, string>();
                    foreach (var pair in answer.Split(';'))
                    {
                        var parts = pair.Split(':');
                        if (parts.Length == 2)
                        {
                            matchDict[parts[0].Trim()] = parts[1].Trim();
                        }
                    }
                    return JsonConvert.SerializeObject(matchDict);

                case "ORDERING":
                    // Format: "Item1|Item2|Item3"
                    var orderItems = answer.Split('|');
                    return JsonConvert.SerializeObject(new { order = orderItems });

                case "ESSAY":
                    // Format: "essay text"
                    return JsonConvert.SerializeObject(new { essay = answer });

                case "SPEAK_PROMPT":
                    // Format: "audio_path.mp3"
                    return JsonConvert.SerializeObject(new { audioPath = answer });

                default:
                    return JsonConvert.SerializeObject(new { answer = answer });
            }
        }

        /// <summary>
        /// Lấy TestSection với tất cả Questions
        /// </summary>
        public TestSectionDTO GetTestSectionWithQuestions(long sectionId)
        {
            TestSectionDTO section = _sectionDAL.GetTestSectionById(sectionId);

            if (section != null)
            {
                section.Questions = _questionDAL.GetQuestionsBySectionIdDTO(sectionId);

                // Load answer keys cho từng question
                foreach (var question in section.Questions)
                {
                    var answerKey = _answerKeyDAL.GetAnswerKeyByQuestionId(question.Id);
                    if (answerKey != null)
                    {
                        question.AnswerData = answerKey.AnswerData;
                    }
                }
            }

            return section;
        }

        /// <summary>
        /// Lấy tất cả sections của một paper
        /// </summary>
        public List<TestSectionDTO> GetTestSectionsByPaperId(long paperId)
        {
            return _sectionDAL.GetTestSectionsByPaperId(paperId);
        }

        /// <summary>
        /// Xóa TestSection (cascade delete questions)
        /// </summary>
        public bool DeleteTestSection(long sectionId)
        {
            return _sectionDAL.DeleteTestSection(sectionId);
        }

        /// <summary>
        /// Update TestSection info
        /// </summary>
        public bool UpdateTestSection(TestSectionDTO section)
        {
            return _sectionDAL.UpdateTestSection(section);
        }

        /// <summary>
        /// Delete a question
        /// </summary>
        public bool DeleteQuestion(long questionId)
        {
            return _questionDAL.DeleteQuestion(questionId);
        }

        /// <summary>
        /// Save questions to existing section
        /// </summary>
        public bool SaveQuestionsToSection(long sectionId,
    Dictionary<int, string> questionTypes,
    Dictionary<int, string> answers,
    Dictionary<int, int> questionRanges)
        {
            try
            {
                // 1. Tìm các đoạn liên tiếp (start - end)
                var ranges = FindContinuousRanges(questionTypes);

                // 2. Tạo map vị trí -> (start, end) để tra nhanh
                Dictionary<int, (int start, int end)> rangeMap = new Dictionary<int, (int, int)>();
                foreach (var r in ranges)
                {
                    for (int i = r.start; i <= r.end; i++)
                    {
                        rangeMap[i] = (r.start, r.end);
                    }
                }

                // 3. Lưu từng Question + Answer
                foreach (var kvp in questionTypes)
                {
                    int position = kvp.Key;
                    string questionType = kvp.Value;

                    if (string.IsNullOrEmpty(questionType))
                        continue;

                    string answer = answers.ContainsKey(position) ? answers[position] : "";

                    // Xác định QuestionText và Points
                    int start = position;
                    int end = position;
                    decimal points = 1;
                    string text = $"Question {position}";

                    if (rangeMap.ContainsKey(position))
                    {
                        start = rangeMap[position].start;
                        end = rangeMap[position].end;

                        text = $"Question {start} - {end}";
                        points = end - start + 1;
                    }

                    // Tạo Question
                    QuestionDTO question = new QuestionDTO
                    {
                        SectionId = sectionId,
                        QuestionType = questionType,
                        QuestionText = text,
                        Points = points,
                        Position = position
                    };

                    long questionId = _questionDAL.InsertQuestion(question);
                    if (questionId <= 0)
                        continue;

                    // Save AnswerKey đúng format JSON
                    if (!string.IsNullOrEmpty(answer))
                    {
                        QuestionAnswerKeyDTO answerKey = new QuestionAnswerKeyDTO
                        {
                            QuestionId = questionId,
                            AnswerData = ConvertAnswerToJson(questionType, answer)
                        };

                        _answerKeyDAL.InsertAnswerKey(answerKey);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving questions to section: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Lấy câu trả lời riêng cho 1 vị trí (pos) trong trường hợp answer là group answer.
        /// - questionType: kiểu câu hỏi (dùng để xử lý đặc thù nếu cần)
        /// - answer: chuỗi câu trả lời group (ví dụ "A|B|C" hoặc "1:A;2:B;3:C" hoặc "A;B;C")
        /// - pos: vị trí hiện tại (ví dụ 5)
        /// - startPosition: vị trí bắt đầu nhóm (ví dụ 3) -> index trong array = pos - startPosition
        /// </summary>
        private static string ExtractIndividualAnswer(string questionType, string answer, int pos, int startPosition)
        {
            if (string.IsNullOrWhiteSpace(answer))
                return string.Empty;

            answer = answer.Trim();

            // 1) Format kiểu "1:Ans;2:Ans;3:Ans" (keyed)
            if (answer.Contains(":"))
            {
                try
                {
                    var pairs = answer.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var dict = new Dictionary<int, string>();
                    foreach (var p in pairs)
                    {
                        var parts = p.Split(new[] { ':' }, 2);
                        if (parts.Length != 2) continue;
                        if (int.TryParse(parts[0].Trim(), out int key))
                        {
                            dict[key] = parts[1].Trim();
                        }
                        else
                        {
                            // nếu key không phải số thì bỏ qua hoặc dùng as-is (không map)
                        }
                    }

                    if (dict.Count > 0)
                    {
                        if (dict.ContainsKey(pos))
                            return dict[pos];
                        // nếu không có key pos, cũng thử key = pos - startPosition + 1 (nếu người dùng đánh số từ 1..n)
                        int relative = pos - startPosition + 1;
                        if (dict.ContainsKey(relative))
                            return dict[relative];
                    }
                }
                catch
                {
                    // ignore parse errors, fallback tiếp
                }
            }

            // 2) Format phân tách theo thứ tự: '|' hoặc ';' hoặc ',' (ưu tiên '|' rồi ';' rồi ',')
            char[] separators = new[] { '|', ';', ',' };
            foreach (var sep in separators)
            {
                if (answer.Contains(sep))
                {
                    var parts = answer.Split(new[] { sep }, StringSplitOptions.None)
                                      .Select(p => p.Trim()).ToArray();

                    int index = pos - startPosition; // zero-based index
                    if (index >= 0 && index < parts.Length)
                        return parts[index];

                    // nếu parts length = pos (dùng vị trí tuyệt đối)
                    if (pos - 1 >= 0 && pos - 1 < parts.Length)
                        return parts[pos - 1];

                    // không tìm thấy -> fallback
                    return string.Empty;
                }
            }

            // 3) Nếu không có dấu phân tách: trả nguyên chuỗi (dùng cho trường hợp không phải group)
            return answer;
        }

        /// <summary>
        /// Chuyển một câu trả lời (đã là individual answer) thành JSON để lưu AnswerKey.
        /// Trả về JSON string (nhỏ gọn) tùy theo questionType.
        /// </summary>
        private static string ConvertAnswerToJson(string questionType, string answer, bool isGroupQuestion)
        {
            // Normalize input
            answer = answer ?? string.Empty;

            // Build a simple object depending on type
            object payload;

            switch ((questionType ?? "").ToUpperInvariant())
            {
                case "MCQ":
                    // single choice -> store single char/string
                    payload = new { type = "MCQ", answer = answer.Trim() };
                    break;

                case "MULTI_SELECT":
                    // multi-select expected as CSV like "A,C,E" or "A|C|E"
                    var separators = new[] { ',', '|', ';' };
                    var items = answer.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
                    payload = new { type = "MULTI_SELECT", answers = items };
                    break;

                case "MATCHING":
                    // matching might be "Left:Right;Left2:Right2" or for individual answer just "Right"
                    if (answer.Contains(":"))
                    {
                        var pairs = answer.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(p =>
                                          {
                                              var parts = p.Split(new[] { ':' }, 2);
                                              return new { left = parts[0].Trim(), right = (parts.Length > 1 ? parts[1].Trim() : "") };
                                          }).ToArray();
                        payload = new { type = "MATCHING", pairs = pairs };
                    }
                    else
                    {
                        payload = new { type = "MATCHING", answer = answer.Trim() };
                    }
                    break;

                case "ORDERING":
                    // ordering stored as array splitted by '|' or ',' or ';'
                    var ordSeps = new[] { '|', ',', ';' };
                    var ordItems = answer.Split(ordSeps, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
                    payload = new { type = "ORDERING", order = ordItems };
                    break;

                case "FILL_BLANK":
                case "SHORT_ANSWER":
                case "ESSAY":
                    payload = new { type = questionType.ToUpperInvariant(), text = answer };
                    break;

                case "SPEAK_PROMPT":
                    payload = new { type = "SPEAK_PROMPT", audioPath = answer };
                    break;

                default:
                    // fallback generic
                    payload = new { type = questionType ?? "UNKNOWN", raw = answer };
                    break;
            }

            // Serialize to JSON
            var options = new JsonSerializerOptions { WriteIndented = false };
            return System.Text.Json.JsonSerializer.Serialize(payload, options);
        }

    }
}
