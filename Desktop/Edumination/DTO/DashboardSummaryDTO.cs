using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DTO
{
    public class DashboardSummaryDTO
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalTestsTaken { get; set; } // Tổng số lượt thi
        public decimal TotalRevenue { get; set; } // Tổng doanh thu
    }

    public class RevenueChartDTO
    {
        public string Month { get; set; } // Ví dụ: "11/2025"
        public decimal Revenue { get; set; }
    }
}
