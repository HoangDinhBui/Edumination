using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DAL
{

    public class MockTestDAL
    {
        public List<MockTestDTO> GetAll()
        {
            List<MockTestDTO> list = new();

            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = @"
            SELECT Id, Year, Title, Description,
                   IsPublished, CreatedBy, CreatedAt
            FROM MockTests
            ORDER BY Year DESC
        ";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();

            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new MockTestDTO
                {
                    Id = r.GetInt64(r.GetOrdinal("Id")),
                    Year = r.GetInt32(r.GetOrdinal("Year")),
                    Title = r.GetString(r.GetOrdinal("Title")),
                    Description = r.IsDBNull(r.GetOrdinal("Description"))
                                    ? null
                                    : r.GetString(r.GetOrdinal("Description")),
                    IsPublished = r.GetBoolean(r.GetOrdinal("IsPublished")),
                    CreatedBy = r.GetInt64(r.GetOrdinal("CreatedBy")),
                    CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt"))
                });
            }

            return list;
        }

        public MockTestDTO? GetById(long id)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = "SELECT * FROM MockTests WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();

            if (!r.Read()) return null;

            return new MockTestDTO
            {
                Id = r.GetInt64(r.GetOrdinal("Id")),
                Year = r.GetInt32(r.GetOrdinal("Year")),
                Title = r.GetString(r.GetOrdinal("Title")),
                Description = r.IsDBNull(r.GetOrdinal("Description"))
                                ? null
                                : r.GetString(r.GetOrdinal("Description")),
                IsPublished = r.GetBoolean(r.GetOrdinal("IsPublished")),
                CreatedBy = r.GetInt64(r.GetOrdinal("CreatedBy")),
                CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt"))
            };
        }

        public long Create(MockTestDTO mock)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = @"
            INSERT INTO MockTests
            (Year, Title, Description, CreatedBy)
            OUTPUT INSERTED.Id
            VALUES
            (@Year, @Title, @Desc, @CreatedBy)
        ";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Year", mock.Year);
            cmd.Parameters.AddWithValue("@Title", mock.Title);
            cmd.Parameters.AddWithValue("@Desc", (object?)mock.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", mock.CreatedBy);

            conn.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        public long Update(MockTestDTO mock)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = @"
            UPDATE MockTests
            SET Year = @Year,
                Title = @Title,
                Description = @Desc,
                IsPublished = @IsPublished
            WHERE Id = @Id
        ";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", mock.Id);
            cmd.Parameters.AddWithValue("@Year", mock.Year);
            cmd.Parameters.AddWithValue("@Title", mock.Title);
            cmd.Parameters.AddWithValue("@Desc", (object?)mock.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsPublished", mock.IsPublished);

            conn.Open();
            cmd.ExecuteNonQuery();
            return mock.Id;
        }

        public bool Delete(long id)
        {
            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = "DELETE FROM MockTests WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }

}

