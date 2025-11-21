using System;
using System.Collections.Generic;

namespace Edumination.WinForms.UI.Forms.TestTaking.ReadingTest
{
    public enum QuestionType
    {
        ShortAnswer,
        TrueFalse
    }

    public class ReadingQuestion
    {
        public int Number { get; set; }
        public string Prompt { get; set; }
        public QuestionType Type { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class ReadingPart
    {
        public int PartId { get; set; }
        public string PartName { get; set; }      // "Part 1", "Part 2", ...
        public string PassageTitle { get; set; }
        public string PassageText { get; set; }
        public List<ReadingQuestion> Questions { get; set; } = new();
    }

    /// <summary>Trạng thái toàn bài Reading Test, lưu Part + câu trả lời.</summary>
    public class ReadingTestState
    {
        public List<ReadingPart> Parts { get; set; } = new();
        public int CurrentPartIndex { get; set; } = 0;
        public Dictionary<int, string> UserAnswers { get; set; } = new();
        public int RemainingSeconds { get; set; }
    }

    public static class ReadingMockData
    {
        public const int TotalTimeSeconds = 60 * 60; // 60 phút

        public static List<ReadingPart> GetParts()
        {
            var parts = new List<ReadingPart>();

            // ===== PART 1 =====
            parts.Add(new ReadingPart
            {
                PartId = 1,
                PartName = "Part 1",
                PassageTitle = "THE HISTORY OF COFFEE",
                PassageText =
@"Coffee is one of the most popular beverages in the world. 
It is believed to have originated in Ethiopia before spreading 
to the Middle East, Europe and the Americas. Over centuries, 
coffee houses became important social and intellectual hubs.",

                Questions = new List<ReadingQuestion>
                {
                    new ReadingQuestion
                    {
                        Number = 1,
                        Prompt = "In which country is coffee believed to have originated?",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "ethiopia"
                    },
                    new ReadingQuestion
                    {
                        Number = 2,
                        Prompt = "Name one region where coffee later spread to.",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "europe"
                    },
                    new ReadingQuestion
                    {
                        Number = 3,
                        Prompt = "True/False: Coffee houses were important social places.",
                        Type = QuestionType.TrueFalse,
                        CorrectAnswer = "true"
                    },
                    new ReadingQuestion
                    {
                        Number = 4,
                        Prompt = "What type of hubs did coffee houses become?",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "social and intellectual"
                    },
                    new ReadingQuestion
                    {
                        Number = 5,
                        Prompt = "True/False: Coffee first appeared in the Americas.",
                        Type = QuestionType.TrueFalse,
                        CorrectAnswer = "false"
                    }
                }
            });

            // ===== PART 2 =====
            parts.Add(new ReadingPart
            {
                PartId = 2,
                PartName = "Part 2",
                PassageTitle = "URBAN GREEN SPACES",
                PassageText =
@"Urban green spaces, such as parks and gardens, are essential for 
the health and well-being of city residents. These areas provide 
opportunities for exercise, relaxation, and social interaction.",

                Questions = new List<ReadingQuestion>
                {
                    new ReadingQuestion
                    {
                        Number = 6,
                        Prompt = "Give one example of an urban green space.",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "park"
                    },
                    new ReadingQuestion
                    {
                        Number = 7,
                        Prompt = "True/False: Green spaces help improve city residents' health.",
                        Type = QuestionType.TrueFalse,
                        CorrectAnswer = "true"
                    },
                    new ReadingQuestion
                    {
                        Number = 8,
                        Prompt = "Name one benefit of urban green spaces.",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "exercise"
                    },
                    new ReadingQuestion
                    {
                        Number = 9,
                        Prompt = "True/False: Urban gardens are not considered green spaces.",
                        Type = QuestionType.TrueFalse,
                        CorrectAnswer = "false"
                    },
                    new ReadingQuestion
                    {
                        Number = 10,
                        Prompt = "Urban green spaces support what type of interaction?",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "social interaction"
                    }
                }
            });

            // ===== PART 3 =====
            parts.Add(new ReadingPart
            {
                PartId = 3,
                PartName = "Part 3",
                PassageTitle = "THE FUTURE OF ELECTRIC CARS",
                PassageText =
@"Electric cars are expected to play a major role in reducing carbon 
emissions. Advances in battery technology are making them more efficient, 
while governments around the world are offering incentives to encourage 
their adoption.",

                Questions = new List<ReadingQuestion>
                {
                    new ReadingQuestion
                    {
                        Number = 11,
                        Prompt = "What type of emissions can electric cars help reduce?",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "carbon emissions"
                    },
                    new ReadingQuestion
                    {
                        Number = 12,
                        Prompt = "True/False: Battery technology is becoming less efficient.",
                        Type = QuestionType.TrueFalse,
                        CorrectAnswer = "false"
                    },
                    new ReadingQuestion
                    {
                        Number = 13,
                        Prompt = "Who is offering incentives to encourage electric car use?",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "governments"
                    },
                    new ReadingQuestion
                    {
                        Number = 14,
                        Prompt = "True/False: Electric cars are expected to disappear in the future.",
                        Type = QuestionType.TrueFalse,
                        CorrectAnswer = "false"
                    },
                    new ReadingQuestion
                    {
                        Number = 15,
                        Prompt = "What technology is improving to make electric cars more efficient?",
                        Type = QuestionType.ShortAnswer,
                        CorrectAnswer = "battery technology"
                    }
                }
            });

            return parts;
        }
    }
}
