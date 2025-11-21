using IELTS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class SectionAttemptBLL
    {
        private SectionAttemptDAL sectionAttemptDAL = new SectionAttemptDAL();
        private AnswerDAL answerDAL = new AnswerDAL();

        public long CreateSectionAttempt(long testAttemptId, long sectionId)
        {
            if (testAttemptId <= 0 || sectionId <= 0)
                throw new Exception("Test Attempt ID hoặc Section ID không hợp lệ!");

            return sectionAttemptDAL.CreateSectionAttempt(testAttemptId, sectionId);
        }

        public bool GradeSectionAttempt(long sectionAttemptId)
        {
            if (sectionAttemptId <= 0)
                throw new Exception("Section Attempt ID không hợp lệ!");

            // Lấy tất cả answers
            DataTable answers = answerDAL.GetAnswersBySectionAttemptId(sectionAttemptId);

            decimal rawScore = 0;
            foreach (DataRow row in answers.Rows)
            {
                if (row["IsCorrect"] != DBNull.Value && Convert.ToBoolean(row["IsCorrect"]))
                {
                    if (row["Score"] != DBNull.Value)
                        rawScore += Convert.ToDecimal(row["Score"]);
                    else
                        rawScore += 1; // Default 1 điểm
                }
            }

            // Giả sử quy đổi đơn giản: rawScore / 5 = band (có thể tham chiếu BandScales table)
            decimal bandScore = Math.Min(9.0m, Math.Max(0.0m, Math.Round(rawScore / 5 * 2, MidpointRounding.AwayFromZero) / 2));

            return sectionAttemptDAL.UpdateSectionScore(sectionAttemptId, rawScore, bandScore);
        }

        public DataTable GetSectionAttemptsByTestAttemptId(long testAttemptId)
        {
            if (testAttemptId <= 0)
                throw new Exception("Test Attempt ID không hợp lệ!");

            return sectionAttemptDAL.GetSectionAttemptsByTestAttemptId(testAttemptId);
        }
    }
}
