using IELTS.DAL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class DashboardBLL
    {
        private readonly DashboardDAL _dashboardDAL;

        public DashboardBLL()
        {
            //string connectionString = DatabaseConnection.GetConnection();
            _dashboardDAL = new DashboardDAL();
        }

        public DashboardStatisticsDTO GetDashboardData()
        {
            return _dashboardDAL.GetDashboardStatistics();
        }
    }
}
