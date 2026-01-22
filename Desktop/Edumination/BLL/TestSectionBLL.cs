using IELTS.DAL;
using IELTS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        #region Public Methods

        public DataTable GetSectionsByPaperId(long paperId)
        {
            if (paperId <= 0)
                throw new Exception("Paper ID không hợp lệ!");

            return _sectionDAL.GetSectionsByPaperId(paperId);
        }

        /// <summary>
        /// Tạo TestSection và trả về ID
        /// </summary>
        public long CreateTestSection(TestSectionDTO section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (section.PaperId <= 0)
                throw new Exception("Paper ID không hợp lệ!");

            if (string.IsNullOrWhiteSpace(section.Skill))
                throw new Exception("Skill không được để trống!");

            return _sectionDAL.InsertTestSection(section);
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

                // 2. Build questionRanges từ questionTypes
                var questionRanges = BuildQuestionRanges(questionTypes);

                // 3. Lưu questions
                return SaveQuestionsToSection(sectionId, questionTypes, answers, questionRanges);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving test section: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Save questions to existing section (dùng cho cả nhập tay và AI Analysis)
        /// </summary>
        public bool SaveQuestionsToSection(long sectionId,
            Dictionary<int, string> questionTypes,
            Dictionary<int, string> answers,
            Dictionary<int, int> questionRanges)
        {
            try
            {
                if (sectionId <= 0)
                    throw new Exception("Section ID không hợp lệ!");

                if (questionTypes == null || questionTypes.Count == 0)
                    return false;

                // Tạo map để tra cứu nhanh range của mỗi câu hỏi
                var rangeMap = BuildRangeMap(questionRanges);

                // Track những câu đã xử lý (để tránh trùng lặp cho các câu hỏi nhóm)
                HashSet<int> processedPositions = new HashSet<int>();

                // Lưu từng Question + Answer
                foreach (var kvp in questionTypes.OrderBy(x => x.Key))
                {
                    int position = kvp.Key;
                    string questionType = kvp.Value;

                    if (string.IsNullOrEmpty(questionType))
                        continue;

                    // Skip nếu đã xử lý trong một nhóm câu hỏi
                    if (processedPositions.Contains(position))
                        continue;

                    string answer = answers.ContainsKey(position) ? answers[position] : "";

                    // Xác định thông tin câu hỏi
                    int start = position;
                    int end = position;
                    decimal points = 1;
                    string text = $"Question {position}";

                    // Kiểm tra nếu là nhóm câu hỏi (Group Question)
                    if (rangeMap.ContainsKey(position))
                    {
                        start = rangeMap[position].start;
                        end = rangeMap[position].end;
                        text = $"Question {start} - {end}";
                        points = end - start + 1;

                        // Đánh dấu tất cả vị trí trong nhóm là đã xử lý
                        for (int i = start; i <= end; i++)
                        {
                            processedPositions.Add(i);
                        }
                    }
                    else
                    {
                        processedPositions.Add(position);
                    }

                    // Tạo Question DTO
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

                    // Lưu AnswerKey theo định dạng JSON chuẩn của hệ thống
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

        public TestSectionDTO GetTestSectionWithQuestions(long sectionId)
        {
            TestSectionDTO section = _sectionDAL.GetTestSectionById(sectionId);

            if (section != null)
            {
                section.Questions = _questionDAL.GetQuestionsBySectionIdDTO(sectionId);

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

        public List<TestSectionDTO> GetTestSectionsByPaperId(long paperId)
        {
            return _sectionDAL.GetTestSectionsByPaperId(paperId);
        }

        public bool DeleteTestSection(long sectionId)
        {
            return _sectionDAL.DeleteTestSection(sectionId);
        }

        public bool UpdateTestSection(TestSectionDTO section)
        {
            return _sectionDAL.UpdateTestSection(section);
        }

        public bool DeleteQuestion(long questionId)
        {
            return _questionDAL.DeleteQuestion(questionId);
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Tự động phát hiện các nhóm câu hỏi (Matching, Multi-select, v.v.)
        /// </summary>
        private Dictionary<int, int> BuildQuestionRanges(Dictionary<int, string> questionTypes)
        {
            var ranges = new Dictionary<int, int>();
            var groupTypes = new[] { "MULTI_SELECT", "MATCHING", "ORDERING" };

            int? groupStart = null;
            string currentGroupType = null;

            var sortedPositions = questionTypes.Keys.OrderBy(k => k).ToList();

            for (int i = 0; i < sortedPositions.Count; i++)
            {
                int pos = sortedPositions[i];
                string type = questionTypes[pos];

                bool isGroupType = groupTypes.Contains(type);

                if (isGroupType)
                {
                    if (groupStart == null)
                    {
                        groupStart = pos;
                        currentGroupType = type;
                    }
                    else if (currentGroupType != type)
                    {
                        if (groupStart.HasValue)
                        {
                            ranges[groupStart.Value] = sortedPositions[i - 1];
                        }
                        groupStart = pos;
                        currentGroupType = type;
                    }
                }
                else
                {
                    if (groupStart.HasValue)
                    {
                        ranges[groupStart.Value] = sortedPositions[i - 1];
                        groupStart = null;
                        currentGroupType = null;
                    }
                }
            }

            if (groupStart.HasValue && sortedPositions.Count > 0)
            {
                ranges[groupStart.Value] = sortedPositions[sortedPositions.Count - 1];
            }

            return ranges;
        }

        private Dictionary<int, (int start, int end)> BuildRangeMap(Dictionary<int, int> questionRanges)
        {
            var rangeMap = new Dictionary<int, (int start, int end)>();

            if (questionRanges == null)
                return rangeMap;

            foreach (var range in questionRanges)
            {
                int start = range.Key;
                int end = range.Value;

                for (int i = start; i <= end; i++)
                {
                    rangeMap[i] = (start, end);
                }
            }

            return rangeMap;
        }

        /// <summary>
        /// Chuẩn hóa đáp án thành chuỗi JSON để lưu xuống database
        /// </summary>
        private string ConvertAnswerToJson(string questionType, string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                answer = "";

            answer = answer.Trim();

            try
            {
                switch (questionType?.ToUpperInvariant())
                {
                    case "MCQ":
                        return JsonConvert.SerializeObject(new { answer = answer });

                    case "MULTI_SELECT":
                        var multiAnswers = answer.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(s => s.Trim())
                                                 .Where(s => !string.IsNullOrEmpty(s))
                                                 .ToArray();
                        return JsonConvert.SerializeObject(new { answers = multiAnswers });

                    case "FILL_BLANK":
                    case "SHORT_ANSWER":
                        return JsonConvert.SerializeObject(new { answer = answer });

                    case "MATCHING":
                        var matchDict = new Dictionary<string, string>();
                        if (answer.Contains(";") && answer.Contains(":"))
                        {
                            foreach (var pair in answer.Split(';'))
                            {
                                var parts = pair.Split(':');
                                if (parts.Length == 2)
                                {
                                    matchDict[parts[0].Trim()] = parts[1].Trim();
                                }
                            }
                            return JsonConvert.SerializeObject(matchDict);
                        }
                        return JsonConvert.SerializeObject(new { answer = answer });

                    case "ORDERING":
                        var orderItems = answer.Split(new[] { '|', ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(s => s.Trim())
                                               .Where(s => !string.IsNullOrEmpty(s))
                                               .ToArray();
                        return JsonConvert.SerializeObject(new { order = orderItems });

                    case "ESSAY":
                        return JsonConvert.SerializeObject(new { essay = answer });

                    case "SPEAK_PROMPT":
                        return JsonConvert.SerializeObject(new { audioPath = answer });

                    default:
                        return JsonConvert.SerializeObject(new { answer = answer });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { answer = answer, error = ex.Message });
            }
        }

        #endregion
    }
}