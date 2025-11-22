using IELTS.UI.User.TestTaking.ReadingTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.UI.User.TestTaking.ListeningTest
{
    internal static class ListeningMockData
    {
        // 40 phút
        public const int TotalTimeSeconds = 40 * 60;

        public static List<ReadingPart> GetParts()
        {
            var parts = new List<ReadingPart>();

            // ===== PART 1 =====
            parts.Add(new ReadingPart
            {
                PartName = "Part 1",
                PassageTitle = "Campus Orientation Talk",
                PassageText =
@"You will hear a student called Alex giving new students a tour of the campus.
He explains where the main buildings are, how to find the library, 
and where students usually meet after class.

The tour begins at the main gate and continues towards the science block...",
                Questions = new List<ReadingQuestion>
                {
                    new ReadingQuestion
                    {
                        Number = 1,
                        Prompt  = "Where does the tour begin?",
                        Type    = QuestionType.ShortAnswer,
                        CorrectAnswer = "main gate"
                    },
                    new ReadingQuestion
                    {
                        Number = 2,
                        Prompt  = "Name ONE place where students usually meet after class.",
                        Type    = QuestionType.ShortAnswer,
                        CorrectAnswer = "cafeteria"
                    },
                    new ReadingQuestion
                    {
                        Number = 3,
                        Prompt  = "True/False: The speaker is a university teacher.",
                        Type    = QuestionType.TrueFalse,
                        CorrectAnswer = "false"
                    },
                    new ReadingQuestion
                    {
                        Number = 4,
                        Prompt  = "What block do science students use most often?",
                        Type    = QuestionType.ShortAnswer,
                        CorrectAnswer = "science block"
                    },
                    new ReadingQuestion
                    {
                        Number = 5,
                        Prompt  = "True/False: The library is closed on weekends.",
                        Type    = QuestionType.TrueFalse,
                        CorrectAnswer = "false"
                    }
                }
            });

            // ===== PART 2 =====
            parts.Add(new ReadingPart
            {
                PartName = "Part 2",
                PassageTitle = "Radio Programme about Sleep",
                PassageText =
@"You will hear part of a radio programme in which a scientist talks about sleep.
He describes how many hours teenagers really need, why phones affect sleep,
and gives advice on how to fall asleep faster.",
                Questions = new List<ReadingQuestion>
                {
                    new ReadingQuestion
                    {
                        Number = 6,
                        Prompt  = "According to the speaker, how many hours of sleep do teenagers need?",
                        Type    = QuestionType.ShortAnswer,
                        CorrectAnswer = "8 hours"
                    },
                    new ReadingQuestion
                    {
                        Number = 7,
                        Prompt  = "True/False: Using phones before bed makes it harder to sleep.",
                        Type    = QuestionType.TrueFalse,
                        CorrectAnswer = "true"
                    },
                    new ReadingQuestion
                    {
                        Number = 8,
                        Prompt  = "Name ONE piece of advice for falling asleep faster.",
                        Type    = QuestionType.ShortAnswer,
                        CorrectAnswer = "turn off screens"
                    },
                    new ReadingQuestion
                    {
                        Number = 9,
                        Prompt  = "True/False: The scientist recommends drinking coffee late at night.",
                        Type    = QuestionType.TrueFalse,
                        CorrectAnswer = "false"
                    },
                    new ReadingQuestion
                    {
                        Number = 10,
                        Prompt  = "What kind of programme is the recording from?",
                        Type    = QuestionType.ShortAnswer,
                        CorrectAnswer = "radio programme"
                    }
                }
            });

            return parts;
        }
    }
}
