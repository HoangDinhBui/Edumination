using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.UI.User
{
    internal class SessionManager
    {
        private static DataRow _userRow;
        private static string _token;

        public static void Login(DataRow row) => _userRow = row;
        public static void SetToken(string token) => _token = token;

        public static void Logout()
        {
            _userRow = null;
            _token = null;
        }

        public static string FullName => _userRow?["FullName"]?.ToString() ?? "Student";
    }
}
