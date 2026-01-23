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

        public List<UserDTO> GetListUsersDTO(string keyword = "")
        {
            var list = new List<UserDTO>();
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT Id, Email, FullName, Role, IsActive, CreatedAt, Phone, DateOfBirth, PasswordHash 
                               FROM Users 
                               WHERE Email LIKE @Key OR FullName LIKE @Key 
                               ORDER BY CreatedAt DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Key", "%" + keyword + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new UserDTO();
                            user.Id = Convert.ToInt64(reader["Id"]);
                            user.Email = reader["Email"].ToString();
                            user.FullName = reader["FullName"].ToString();
                            user.Role = reader["Role"].ToString();
                            user.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            user.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                            user.PasswordHash = reader["PasswordHash"].ToString(); // Cần lấy để verify update

                            if (reader["Phone"] != DBNull.Value) user.Phone = reader["Phone"].ToString();
                            if (reader["DateOfBirth"] != DBNull.Value) user.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);

                            list.Add(user);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// [ADMIN] Thêm user với đầy đủ quyền hạn
        /// </summary>
        public bool Admin_AddUser(UserDTO user)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"INSERT INTO Users (Email, PasswordHash, FullName, Role, IsActive, Phone, DateOfBirth, CreatedAt) 
                  VALUES (@Email, @PasswordHash, @FullName, @Role, @IsActive, @Phone, @Dob, GETDATE())", conn))
            {
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                cmd.Parameters.AddWithValue("@Phone", (object)user.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Dob", (object)user.DateOfBirth ?? DBNull.Value);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// [ADMIN] Cập nhật thông tin user (bao gồm cả Role, Active)
        /// </summary>
        public bool Admin_UpdateUser(UserDTO user, bool isChangePassword)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                string sql;
                if (isChangePassword)
                {
                    sql = @"UPDATE Users 
                            SET FullName=@Name, Email=@Email, PasswordHash=@Pass, Role=@Role, IsActive=@Active, 
                                Phone=@Phone, DateOfBirth=@Dob, UpdatedAt=GETDATE() 
                            WHERE Id=@Id";
                }
                else
                {
                    sql = @"UPDATE Users 
                            SET FullName=@Name, Email=@Email, Role=@Role, IsActive=@Active, 
                                Phone=@Phone, DateOfBirth=@Dob, UpdatedAt=GETDATE() 
                            WHERE Id=@Id";
                }

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@Name", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@Active", user.IsActive);
                    cmd.Parameters.AddWithValue("@Phone", (object)user.Phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Dob", (object)user.DateOfBirth ?? DBNull.Value);

                    if (isChangePassword)
                        cmd.Parameters.AddWithValue("@Pass", user.PasswordHash);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// [ADMIN] Xóa user
        /// </summary>
        public bool DeleteUser(long id)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<UserDTO> GetStudentsByCourse(long courseId, string keyword = "")
        {
            var list = new List<UserDTO>();

            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT 
                u.Id,
                u.Email,
                u.FullName,
                u.Phone,
                u.DateOfBirth,
                u.Role,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt
            FROM CourseStudents cs
            JOIN Users u ON u.Id = cs.StudentId
            WHERE cs.CourseId = @CourseId
              AND u.FullName LIKE @Key
            ORDER BY u.FullName";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@Key", "%" + keyword + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new UserDTO
                            {
                                Id = Convert.ToInt64(reader["Id"]),
                                Email = reader["Email"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Phone = reader["Phone"]?.ToString(),
                                DateOfBirth = reader["DateOfBirth"] as DateTime?,
                                Role = reader["Role"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
