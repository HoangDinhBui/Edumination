using IELTS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    internal class CourseStudentBLL
    {
        private readonly CourseStudentDAL dal = new CourseStudentDAL();

        // Danh sách học viên (từ Enrollments)
        public DataTable GetEnrolledUsersByCourse(long courseId, string keyword = "")
        {
            if (courseId <= 0)
                throw new System.Exception("CourseId không hợp lệ");

            return dal.GetEnrolledUsersByCourse(courseId, keyword);
        }

        // Chi tiết học viên (CourseStudents)
        public DataTable GetCourseStudentDetail(long courseId, long studentId)
        {
            if (courseId <= 0 || studentId <= 0)
                throw new System.Exception("Dữ liệu không hợp lệ");

            return dal.GetCourseStudentDetail(courseId, studentId);
        }
    }
}
