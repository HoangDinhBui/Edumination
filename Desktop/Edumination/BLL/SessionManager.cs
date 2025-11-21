using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public static class SessionManager
    {
        public static long CurrentUserId { get; set; }
        public static string CurrentUserEmail { get; set; }
        public static string CurrentUserName { get; set; }
        public static string CurrentUserRole { get; set; }

        public static bool IsLoggedIn
        {
            get { return CurrentUserId > 0; }
        }

        public static bool IsAdmin
        {
            get { return CurrentUserRole == "ADMIN"; }
        }

        public static bool IsTeacher
        {
            get { return CurrentUserRole == "TEACHER"; }
        }

        public static bool IsStudent
        {
            get { return CurrentUserRole == "STUDENT"; }
        }

        public static void Login(DataRow user)
        {
            CurrentUserId = Convert.ToInt64(user["Id"]);
            CurrentUserEmail = user["Email"].ToString();
            CurrentUserName = user["FullName"].ToString();
            CurrentUserRole = user["Role"].ToString();
        }

        public static void Logout()
        {
            CurrentUserId = 0;
            CurrentUserEmail = null;
            CurrentUserName = null;
            CurrentUserRole = null;
        }
    }
}
