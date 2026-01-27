using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DAL
{
    public class PassageDAL
    {
        public List<PassageDTO> GetBySectionId(long sectionId)
        {
            List<PassageDTO> list = new();

            using SqlConnection conn = DatabaseConnection.GetConnection();
            string sql = @"
                SELECT Id, SectionId, Title, ContentText, Position
                FROM Passages
                WHERE SectionId = @SectionId
                ORDER BY Position
            ";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SectionId", sectionId);

            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new PassageDTO
                {
                    Id = r.GetInt64(0),
                    SectionId = r.GetInt64(1),
                    Title = r.IsDBNull(2) ? null : r.GetString(2),
                    ContentText = r.IsDBNull(3) ? null : r.GetString(3),
                    Position = r.GetInt32(4)
                });
            }

            return list;
        }
    }
}