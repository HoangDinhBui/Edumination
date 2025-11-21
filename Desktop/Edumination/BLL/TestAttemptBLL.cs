using IELTS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class TestAttemptBLL
    {
        private TestAttemptDAL attemptDAL = new TestAttemptDAL();
        private SectionAttemptDAL sectionAttemptDAL = new SectionAttemptDAL();

        public long CreateAttempt(long userId, long paperId)
        {
            if (userId <= 0 || paperId <= 0)
                throw new Exception("User ID hoặc Paper ID không hợp lệ!");

            return attemptDAL.CreateAttempt(userId, paperId);
        }

        public DataTable GetAttemptsByUserId(long userId)
        {
            if (userId <= 0)
                throw new Exception("User ID không hợp lệ!");

            return attemptDAL.GetAttemptsByUserId(userId);
        }

        public bool FinishAttempt(long attemptId)
        {
            if (attemptId <= 0)
                throw new Exception("Attempt ID không hợp lệ!");

            // Tính overall band từ 4 sections
            DataTable sections = sectionAttemptDAL.GetSectionAttemptsByTestAttemptId(attemptId);

            if (sections.Rows.Count == 0)
                throw new Exception("Không có dữ liệu section để tính điểm!");

            decimal totalBand = 0;
            int count = 0;

            foreach (DataRow row in sections.Rows)
            {
                if (row["BandScore"] != DBNull.Value)
                {
                    totalBand += Convert.ToDecimal(row["BandScore"]);
                    count++;
                }
            }

            if (count == 0)
                throw new Exception("Chưa có điểm nào để tính overall band!");

            decimal overallBand = Math.Round(totalBand / count * 2, MidpointRounding.AwayFromZero) / 2; // Làm tròn 0.5

            return attemptDAL.FinishAttempt(attemptId, overallBand);
        }
    }
}
