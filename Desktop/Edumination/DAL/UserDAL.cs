using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IELTS.DAL
{
    public class UserDAL
    {
        /// <summary>
        /// Lấy user theo email (bao gồm PasswordHash) để verify với BCrypt
        /// </summary>
        public DataTable GetUserByEmail(string email)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Email, PasswordHash, FullName, Role, IsActive 
              FROM Users 
              WHERE Email = @Email AND IsActive = 1", conn))
            {
                // Trim email để tránh khoảng trắng
                string trimmedEmail = email?.Trim();
                
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 255).Value = trimmedEmail;

                Console.WriteLine($"🔍 [UserDAL] Tìm kiếm email: '{trimmedEmail}'");

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                    Console.WriteLine($"📊 [UserDAL] Kết quả: {dt.Rows.Count} user(s) tìm thấy");
                    
                    if (dt.Rows.Count > 0)
                    {
                        Console.WriteLine($"✅ [UserDAL] User: {dt.Rows[0]["FullName"]} ({dt.Rows[0]["Email"]})");
                    }
                    
                    return dt;
                }
            }
        }

        /// <summary>
        /// Login method cũ (deprecated - dùng GetUserByEmail thay thế)
        /// Giữ lại để backward compatibility
        /// </summary>
        public DataTable Login(string email, string passwordHash)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT Id, Email, FullName, Role, IsActive 
              FROM Users 
              WHERE Email = @Email AND PasswordHash = @PasswordHash AND IsActive = 1", conn))
            {
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 255).Value = email;
                cmd.Parameters.Add("@PasswordHash", System.Data.SqlDbType.NVarChar, 255).Value = passwordHash;

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public bool Register(string email, string passwordHash, string fullName, string role = "STUDENT")
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Users (Email, PasswordHash, FullName, Role) 
              VALUES (@Email, @PasswordHash, @FullName, @Role)", conn))
            {
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 255).Value = email;
                cmd.Parameters.Add("@PasswordHash", System.Data.SqlDbType.NVarChar, 255).Value = passwordHash;
                cmd.Parameters.Add("@FullName", System.Data.SqlDbType.NVarChar, 255).Value = fullName;
                cmd.Parameters.Add("@Role", System.Data.SqlDbType.NVarChar, 20).Value = role;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //public DataTable GetUserById(long userId)
        //{
        //    using (SqlConnection conn = DatabaseConnection.GetConnection())
        //    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Id = @UserId", conn))
        //    {
        //        cmd.Parameters.Add("@UserId", System.Data.SqlDbType.BigInt).Value = userId;

        //        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        //        {
        //            DataTable dt = new DataTable();
        //            adapter.Fill(dt);
        //            return dt;
        //        }
        //    }
        //}

        public bool UpdateProfile(long userId, string fullName, string phone, DateTime? dob)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Users 
              SET FullName = @FullName, Phone = @Phone, DateOfBirth = @DOB, UpdatedAt = GETDATE()
              WHERE Id = @UserId", conn))
            {
                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.BigInt).Value = userId;
                cmd.Parameters.Add("@FullName", System.Data.SqlDbType.NVarChar, 255).Value = fullName;
                cmd.Parameters.Add("@Phone", System.Data.SqlDbType.NVarChar, 30).Value = (object)phone ?? DBNull.Value;
                cmd.Parameters.Add("@DOB", System.Data.SqlDbType.Date).Value = (object)dob ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public DataTable GetAllUsers()
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Id, Email, FullName, Role, IsActive, CreatedAt FROM Users ORDER BY CreatedAt DESC", conn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public UserDTO GetUserById(long id)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, FullName FROM Users WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            return new UserDTO
                            {
                                Id = r.GetInt64(0),
                                FullName = r.GetString(1)
                            };
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Cập nhật mật khẩu mới (dùng cho forgot password)
        /// </summary>
        public bool UpdatePassword(string email, string newPasswordHash)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Users 
              SET PasswordHash = @PasswordHash, UpdatedAt = GETDATE()
              WHERE Email = @Email AND IsActive = 1", conn))
            {
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar, 255).Value = email;
                cmd.Parameters.Add("@PasswordHash", System.Data.SqlDbType.NVarChar, 255).Value = newPasswordHash;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
