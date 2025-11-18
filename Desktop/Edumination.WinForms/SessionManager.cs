using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// File: SessionManager.cs

namespace Edumination.WinForms
{
    /// <summary>
    /// Lớp tĩnh dùng để lưu trữ dữ liệu phiên (session) của người dùng sau khi đăng nhập.
    /// </summary>
    public static class SessionManager
    {
        // Token JWT nhận được từ server
        public static string JwtToken { get; set; }

        // Vai trò (Role) của người dùng đã được giải mã từ Token
        public static string UserRole { get; set; }

        // UserId hoặc các thông tin khác nếu cần
        // public static string UserId { get; set; }

        /// <summary>
        /// Xóa dữ liệu phiên khi người dùng đăng xuất.
        /// </summary>
        public static void ClearSession()
        {
            JwtToken = null;
            UserRole = null;
        }
    }
}
