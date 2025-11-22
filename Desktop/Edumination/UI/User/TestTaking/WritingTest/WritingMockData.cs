using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.UI.User.TestTaking.WritingTest
{
    public class WritingTask
    {
        public string PartName { get; set; }
        public string Title { get; set; }
        public string Prompt { get; set; }
    }

    internal static class WritingMockData
    {
        public const int TotalTimeSeconds = 60 * 60; // 60 phút

        public static List<WritingTask> GetTasks()
        {
            return new List<WritingTask>
            {
                new WritingTask
                {
                    PartName = "Task 1",
                    Title = "IELTS Writing Task 1 – Graph",
                    Prompt =
@"You should spend about 20 minutes on this task.

The chart below shows the percentage of students who used different transport 
methods to travel to school in a particular city between 1990 and 2020.

Summarise the information by selecting and reporting the main features, 
and make comparisons where relevant.

Write at least 150 words."
                },
                new WritingTask
                {
                    PartName = "Task 2",
                    Title = "IELTS Writing Task 2 – Opinion Essay",
                    Prompt =
@"You should spend about 40 minutes on this task.

Some people think that all university students should study whatever they like.
Others believe they should only be allowed to study subjects that will be useful 
in the future, such as science and technology.

Discuss both these views and give your own opinion.

Write at least 250 words."
                }
            };
        }
    }
}
