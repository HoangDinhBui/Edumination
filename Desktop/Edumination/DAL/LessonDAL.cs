using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Sunny.UI;

namespace IELTS.DAL
{
	// Đổi internal thành public để BLL có thể truy cập
	public class LessonDAL
	{
		private string connectionString = ConfigurationManager.ConnectionStrings["IELTSConnection"].ConnectionString;

		public List<LessonDTO> GetByCourseId(long courseId)
		{
			List<LessonDTO> list = new List<LessonDTO>();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string sql = "SELECT * FROM Lessons WHERE CourseId = @CourseId ORDER BY Position";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@CourseId", courseId);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					list.Add(new LessonDTO
					{
						Id = Convert.ToInt64(dr["Id"]),
						CourseId = Convert.ToInt64(dr["CourseId"]),
						Title = dr["Title"].ToString(),
						VideoFilePath = dr["VideoFilePath"].ToString(),
						Position = Convert.ToInt32(dr["Position"]),
						IsPublished = Convert.ToBoolean(dr["IsPublished"])
					});
				}
			}
			return list;
		}

		public bool Insert(LessonDTO lesson)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlTransaction trans = conn.BeginTransaction();
				try
				{
					string sqlLesson = @"INSERT INTO Lessons (CourseId, Title, VideoFilePath, Position, IsPublished, CreatedBy, CreatedAt) 
                                        OUTPUT INSERTED.Id
                                        VALUES (@CourseId, @Title, @VideoFilePath, @Position, @IsPublished, @CreatedBy, GETDATE())";

					SqlCommand cmdLesson = new SqlCommand(sqlLesson, conn, trans);
					cmdLesson.Parameters.AddWithValue("@CourseId", lesson.CourseId);
					cmdLesson.Parameters.AddWithValue("@Title", lesson.Title);
					cmdLesson.Parameters.AddWithValue("@VideoFilePath", lesson.VideoFilePath);
					cmdLesson.Parameters.AddWithValue("@Position", lesson.Position);
					cmdLesson.Parameters.AddWithValue("@IsPublished", lesson.IsPublished);
					cmdLesson.Parameters.AddWithValue("@CreatedBy", 1);

					long newLessonId = (long)cmdLesson.ExecuteScalar();

					string sqlTest = "INSERT INTO LessonTests (LessonId, Title, CreatedAt) VALUES (@LessonId, @Title, GETDATE())";
					SqlCommand cmdTest = new SqlCommand(sqlTest, conn, trans);
					cmdTest.Parameters.AddWithValue("@LessonId", newLessonId);
					cmdTest.Parameters.AddWithValue("@Title", "Test: " + lesson.Title);

					cmdTest.ExecuteNonQuery();
					trans.Commit();
					return true;
				}
				catch
				{
					trans.Rollback();
					return false;
				}
			}
		}

		// BỔ SUNG PHƯƠNG THỨC UPDATE (BLL đang gọi)
		public bool Update(LessonDTO lesson)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string sql = @"UPDATE Lessons SET Title = @Title, VideoFilePath = @VideoFilePath, 
                             Position = @Position, IsPublished = @IsPublished WHERE Id = @Id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@Title", lesson.Title);
				cmd.Parameters.AddWithValue("@VideoFilePath", lesson.VideoFilePath);
				cmd.Parameters.AddWithValue("@Position", lesson.Position);
				cmd.Parameters.AddWithValue("@IsPublished", lesson.IsPublished);
				cmd.Parameters.AddWithValue("@Id", lesson.Id);
				conn.Open();
				return cmd.ExecuteNonQuery() > 0;
			}
		}

		public bool Delete(long lessonId)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string sql = "DELETE FROM Lessons WHERE Id = @Id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@Id", lessonId);
				conn.Open();
				return cmd.ExecuteNonQuery() > 0;
			}
		}

		public List<LessonTestQuestionDTO> GetQuestionsByLessonId(long lessonId)
		{
			List<LessonTestQuestionDTO> list = new List<LessonTestQuestionDTO>();
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string sql = @"SELECT q.* FROM LessonTestQuestions q
                               JOIN LessonTests t ON q.LessonTestId = t.Id
                               WHERE t.LessonId = @LessonId  ORDER BY q.Position";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@LessonId", lessonId);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					list.Add(new LessonTestQuestionDTO
					{
						Id = Convert.ToInt64(dr["Id"]),
						LessonTestId = Convert.ToInt64(dr["LessonTestId"]),
						QuestionText = dr["QuestionText"].ToString(),
						ChoiceA = dr["ChoiceA"].ToString(),
						ChoiceB = dr["ChoiceB"].ToString(),
						ChoiceC = dr["ChoiceC"].ToString(),
						ChoiceD = dr["ChoiceD"].ToString(),
						CorrectAnswer = dr["CorrectAnswer"].ToString(),
						Position = Convert.ToInt32(dr["Position"])
					});
				}
			}
			return list;
		}

		public LessonDTO GetById(long lessonId)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				string sql = "SELECT * FROM Lessons WHERE Id = @Id";
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.AddWithValue("@Id", lessonId);
				conn.Open();
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.Read())
				{
					return new LessonDTO
					{
						Id = Convert.ToInt64(dr["Id"]),
						CourseId = Convert.ToInt64(dr["CourseId"]),
						Title = dr["Title"].ToString(),
						VideoFilePath = dr["VideoFilePath"].ToString(),
						Position = Convert.ToInt32(dr["Position"]),
						IsPublished = Convert.ToBoolean(dr["IsPublished"])
					};
				}
			}
			return null;
		}

		// BỔ SUNG PHƯƠNG THỨC SAVEQUIZ (Để lưu 10 câu hỏi)
		public bool SaveQuiz(long lessonId, List<LessonTestQuestionDTO> questions)
		{
			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				conn.Open();
				SqlTransaction trans = conn.BeginTransaction();
				try
				{
					// 1. Lấy LessonTestId từ LessonId (Sử dụng object để kiểm tra null)
					string sqlGetTestId = "SELECT Id FROM LessonTests WHERE LessonId = @LessonId";
					SqlCommand cmdGet = new SqlCommand(sqlGetTestId, conn, trans);
					cmdGet.Parameters.AddWithValue("@LessonId", lessonId);

					object result = cmdGet.ExecuteScalar(); // Thực thi an toàn

					// Kiểm tra nếu chưa tồn tại LessonTest cho bài học này
					if (result == null || result == DBNull.Value)
					{
						// Tự động tạo mới LessonTests nếu chưa có
						string sqlCreateTest = "INSERT INTO LessonTests (LessonId, Title, CreatedAt) OUTPUT INSERTED.Id VALUES (@LessonId, @Title, GETDATE())";
						SqlCommand cmdCreate = new SqlCommand(sqlCreateTest, conn, trans);
						cmdCreate.Parameters.AddWithValue("@LessonId", lessonId);
						cmdCreate.Parameters.AddWithValue("@Title", "Bài kiểm tra mặc định");
						result = cmdCreate.ExecuteScalar();
					}

					long testId = Convert.ToInt64(result); // Ép kiểu an toàn sau khi đã kiểm tra

					// 2. Xóa các câu hỏi cũ để thay bằng danh sách mới
					string sqlDel = "DELETE FROM LessonTestQuestions WHERE LessonTestId = @TestId";
					SqlCommand cmdDel = new SqlCommand(sqlDel, conn, trans);
					cmdDel.Parameters.AddWithValue("@TestId", testId);
					cmdDel.ExecuteNonQuery();

					// 3. Thêm danh sách câu hỏi mới
					string sqlIns = @"INSERT INTO LessonTestQuestions (LessonTestId, QuestionText, ChoiceA, ChoiceB, ChoiceC, ChoiceD, CorrectAnswer, Position)
                            VALUES (@TestId, @QText, @A, @B, @C, @D, @Correct, @Pos)";

					// Chỉ thực hiện vòng lặp nếu có câu hỏi truyền vào
					if (questions != null)
					{
						foreach (var q in questions)
						{
							SqlCommand cmdIns = new SqlCommand(sqlIns, conn, trans);
							cmdIns.Parameters.AddWithValue("@TestId", testId);
							cmdIns.Parameters.AddWithValue("@QText", q.QuestionText ?? "");
							cmdIns.Parameters.AddWithValue("@A", q.ChoiceA ?? "");
							cmdIns.Parameters.AddWithValue("@B", q.ChoiceB ?? "");
							cmdIns.Parameters.AddWithValue("@C", q.ChoiceC ?? "");
							cmdIns.Parameters.AddWithValue("@D", q.ChoiceD ?? "");
							cmdIns.Parameters.AddWithValue("@Correct", q.CorrectAnswer ?? "A");
							cmdIns.Parameters.AddWithValue("@Pos", q.Position);
							cmdIns.ExecuteNonQuery();
						}
					}

					trans.Commit();
					return true;
				}
				catch (Exception ex)
				{
					trans.Rollback();
					// Bạn nên log lỗi ex ra để biết nguyên nhân thực sự nếu vẫn lỗi
					return false;
				}
			}
		}
	}
}