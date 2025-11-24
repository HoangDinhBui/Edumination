using IELTS.DTO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.DAL
{
    public class AnswerKeyDAL
    {
        /// <summary>
        /// Thêm AnswerKey
        /// </summary>
        public long InsertAnswerKey(QuestionAnswerKeyDTO answerKey)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO QuestionAnswerKeys (QuestionId, AnswerData)
                    VALUES (@QuestionId, @AnswerData);
                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@QuestionId", answerKey.QuestionId);
                    cmd.Parameters.AddWithValue("@AnswerData", answerKey.AnswerData);

                    return (long)cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Lấy AnswerKey theo QuestionId
        /// </summary>
        public QuestionAnswerKeyDTO GetAnswerKeyByQuestionId(long questionId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT Id, QuestionId, AnswerData
                    FROM QuestionAnswerKeys
                    WHERE QuestionId = @QuestionId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new QuestionAnswerKeyDTO
                            {
                                Id = reader.GetInt64(0),
                                QuestionId = reader.GetInt64(1),
                                AnswerData = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool DeleteByQuestionId(long questionId)
        {
            using (SqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string query = @"DELETE FROM QuestionAnswerKeys WHERE QuestionId = @QuestionId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@QuestionId", questionId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

    }
}
