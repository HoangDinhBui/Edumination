using IELTS.DAL;
using IELTS.DTO;
using System;
using System.Collections.Generic;

namespace IELTS.BLL
{
	// Đổi thành public để các Form trong UI có thể gọi được
	public class LessonBLL
	{
		private readonly LessonDAL _dal = new LessonDAL();

		/// <summary>
		/// Lấy danh sách bài học theo mã khóa học
		/// </summary>
		public List<LessonDTO> GetByCourseId(long courseId)
		{
			return _dal.GetByCourseId(courseId);
		}

		/// <summary>
		/// Lấy chi tiết một bài học (bao gồm cả VideoPath)
		/// </summary>
		public LessonDTO GetById(long lessonId)
		{
			return _dal.GetById(lessonId);
		}

		/// <summary>
		/// Thêm bài học mới (Giao diện gọi để thêm 1 record Lesson vào DB)
		/// </summary>
		public string AddLesson(LessonDTO lesson)
		{
			if (string.IsNullOrWhiteSpace(lesson.Title))
				return "Tên bài học không được để trống.";

			try
			{
				// Gọi xuống DAL để thực hiện Transaction thêm Lesson và LessonTest
				bool result = _dal.Insert(lesson);
				return result ? "" : "Không thể thêm bài học vào cơ sở dữ liệu.";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		/// <summary>
		/// Cập nhật thông tin bài học
		/// </summary>
		public string UpdateLesson(LessonDTO lesson)
		{
			try
			{
				bool result = _dal.Update(lesson); // Đảm bảo bạn đã viết hàm Update trong DAL
				return result ? "" : "Cập nhật thất bại.";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		/// <summary>
		/// Xóa bài học (Sẽ tự động xóa nội dung liên quan nhờ CASCADE)
		/// </summary>
		public bool DeleteLesson(long lessonId)
		{
			return _dal.Delete(lessonId);
		}

		// =========================================================
		// NGHIỆP VỤ CHO BÀI KIỂM TRA (QUIZ) TRONG BÀI HỌC
		// =========================================================

		/// <summary>
		/// Lấy danh sách 10 câu hỏi của một bài học. 
		/// Giải quyết lỗi CS0246 bằng cách dùng LessonTestQuestionDTO
		/// </summary>
		public List<LessonTestQuestionDTO> GetQuestionsByLessonId(long lessonId)
		{
			return _dal.GetQuestionsByLessonId(lessonId);
		}

		/// <summary>
		/// Lưu hoặc cập nhật danh sách câu hỏi trắc nghiệm (Tối đa 10 câu)
		/// Áp dụng quy tắc nghiệp vụ giới hạn số lượng câu hỏi
		/// </summary>
		public string SaveLessonQuiz(long lessonId, List<LessonTestQuestionDTO> questions)
		{
			// Kiểm tra nghiệp vụ: tối đa 10 câu theo thiết kế Database
			if (questions != null && questions.Count > 10)
				return "Mỗi bài học chỉ được phép có tối đa 10 câu hỏi.";

			try
			{
				// Phương thức SaveQuiz sẽ xử lý việc xóa cũ - thêm mới câu hỏi trong DAL
				return _dal.SaveQuiz(lessonId, questions) ? "" : "Lỗi khi lưu bài trắc nghiệm.";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}