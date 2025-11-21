using IELTS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class TestSectionBLL
    {
        private TestSectionDAL sectionDAL = new TestSectionDAL();

        public DataTable GetSectionsByPaperId(long paperId)
        {
            if (paperId <= 0)
                throw new Exception("Paper ID không hợp lệ!");

            return sectionDAL.GetSectionsByPaperId(paperId);
        }

        public long CreateSection(long paperId, string skill, int? timeLimitMinutes, string audioFilePath)
        {
            if (paperId <= 0)
                throw new Exception("Paper ID không hợp lệ!");

            string[] validSkills = { "LISTENING", "READING", "WRITING", "SPEAKING" };
            if (Array.IndexOf(validSkills, skill) == -1)
                throw new Exception("Kỹ năng không hợp lệ! (LISTENING/READING/WRITING/SPEAKING)");

            if (timeLimitMinutes.HasValue && timeLimitMinutes.Value <= 0)
                throw new Exception("Thời gian làm bài phải lớn hơn 0!");

            return sectionDAL.CreateSection(paperId, skill, timeLimitMinutes, audioFilePath);
        }
    }
}
