using IELTS.DAL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class ReportBLL
    {
        private readonly ReportDAL _dal = new ReportDAL();

        public DashboardSummaryDTO GetDashboardSummary()
        {
            return _dal.GetSummary();
        }

        public List<RevenueChartDTO> GetRevenueChart()
        {
            return _dal.GetRevenueLast6Months();
        }
    }
}
