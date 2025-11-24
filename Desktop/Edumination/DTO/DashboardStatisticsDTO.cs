using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class DashboardStatisticsDTO
    {
        public int TotalStudents { get; set; }
        public int TotalTests { get; set; }
        public int TotalCourses { get; set; }
        public int ActiveEnrollments { get; set; }

        // Growth percentages
        public decimal StudentGrowth { get; set; }
        public decimal TestGrowth { get; set; }
        public decimal CourseGrowth { get; set; }
        public decimal EnrollmentGrowth { get; set; }

        // Recent activities
        public List<RecentActivityDTO> RecentActivities { get; set; }

        // Test statistics
        public List<TestStatDTO> TopTests { get; set; }

        // Course statistics
        public List<CourseStatDTO> TopCourses { get; set; }

        // Monthly data for charts
        public List<MonthlyDataDTO> MonthlyStudents { get; set; }
        public List<MonthlyDataDTO> MonthlyTests { get; set; }
    }

    public class RecentActivityDTO
    {
        public string UserName { get; set; }
        public string Activity { get; set; }
        public DateTime Time { get; set; }
        public string Icon { get; set; }
    }

    public class TestStatDTO
    {
        public string TestTitle { get; set; }
        public int AttemptCount { get; set; }
        public decimal AverageBand { get; set; }
    }

    public class CourseStatDTO
    {
        public string CourseTitle { get; set; }
        public int EnrollmentCount { get; set; }
        public int CompletionRate { get; set; }
    }

    public class MonthlyDataDTO
    {
        public string Month { get; set; }
        public int Count { get; set; }
    }
}
