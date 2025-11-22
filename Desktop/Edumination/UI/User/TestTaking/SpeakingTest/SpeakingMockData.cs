using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.UI.User.TestTaking.SpeakingTest
{
    public class SpeakingPart
    {
        public string PartName { get; set; }        // Part 1 / Part 2 / Part 3
        public string Title { get; set; }           // introduction and interview
        public string VideoPath { get; set; }       // mock video
        public List<string> Questions { get; set; }  // danh sách câu hỏi
    }

    public static class SpeakingMockData
    {
        public const int TotalTimeSeconds = 5 * 60;

        public static List<SpeakingPart> GetParts()
        {
            return new List<SpeakingPart>
            {
                new SpeakingPart
                {
                    PartName = "Part 1",
                    Title = "Introduction and Interview",
                    VideoPath = "assets/video/speaking1.mp4", // mock
                    Questions = new()
                    {
                        "Tell me about your hometown.",
                        "What do you do?"
                    }
                },

                new SpeakingPart
                {
                    PartName = "Part 2",
                    Title = "Cue Card",
                    VideoPath = "assets/video/speaking2.mp4",
                    Questions = new()
                    {
                        "Describe a person who inspires you."
                    }
                },

                new SpeakingPart
                {
                    PartName = "Part 3",
                    Title = "Discussion",
                    VideoPath = "assets/video/speaking3.mp4",
                    Questions = new()
                    {
                        "Do you think society benefits from heroes?",
                        "What kind of people become role models?"
                    }
                }
            };
        }
    }
}
