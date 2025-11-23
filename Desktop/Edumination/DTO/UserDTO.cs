using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace IELTS.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Role { get; set; } // STUDENT, TEACHER, ADMIN
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Constructor
        public UserDTO() { }

        public UserDTO(long id, string email, string fullName, string role)
        {
            Id = id;
            Email = email;
            FullName = fullName;
            Role = role;
        }

        // Helper methods
        public bool IsStudent => Role == "STUDENT";
        public bool IsTeacher => Role == "TEACHER";
        public bool IsAdmin => Role == "ADMIN";

        public override string ToString()
        {
            return $"{FullName} ({Email})";
        }
    }

    // =========================================================
    // 2. TEST PAPER DTO
    // =========================================================
    

    // =========================================================
    // 3. TEST SECTION DTO
    // =========================================================
    

    // =========================================================
    // 4. PASSAGE DTO
    // =========================================================
    

    // =========================================================
    // 5. QUESTION DTO
    // =========================================================
    

    // =========================================================
    // 6. QUESTION CHOICE DTO (cho MCQ)
    // =========================================================
    

    // =========================================================
    // 7. QUESTION ANSWER KEY DTO
    // =========================================================
    

    // =========================================================
    // 8. BAND SCALE DTO
    // =========================================================
    

    // =========================================================
    // 9. TEST ATTEMPT DTO
    // =========================================================
    

    // =========================================================
    // 10. SECTION ATTEMPT DTO
    // =========================================================
    

    // =========================================================
    // 11. ANSWER DTO
    // =========================================================
    

    // =========================================================
    // 12. WRITING SUBMISSION DTO
    // =========================================================
    

    // =========================================================
    // 13. SPEAKING SUBMISSION DTO
    // =========================================================
    

    // =========================================================
    // 14. AI EVALUATION DTO
    // =========================================================
    public class AIEvaluationDTO
    {
        public long Id { get; set; }
        public string SubmissionType { get; set; } // WRITING, SPEAKING
        public long SubmissionId { get; set; }
        public long RubricId { get; set; }
        public long ModelId { get; set; }
        public decimal? OverallScore { get; set; }
        public decimal? BandScore { get; set; }
        public string FeedbackSummary { get; set; }
        public string RawResponseJson { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public List<AIEvaluationDetailDTO> Details { get; set; }

        public AIEvaluationDTO()
        {
            Details = new List<AIEvaluationDetailDTO>();
        }

        public bool IsWriting => SubmissionType == "WRITING";
        public bool IsSpeaking => SubmissionType == "SPEAKING";

        public override string ToString()
        {
            return $"{SubmissionType} - Band {BandScore?.ToString() ?? "N/A"}";
        }
    }

    // =========================================================
    // 15. AI EVALUATION DETAIL DTO
    // =========================================================
    public class AIEvaluationDetailDTO
    {
        public long Id { get; set; }
        public long EvaluationId { get; set; }
        public long CriterionId { get; set; }
        public string CriterionCode { get; set; } // FLUENCY, LEXICAL, COHERENCE, etc.
        public string CriterionName { get; set; }
        public decimal Score { get; set; }
        public string FeedbackText { get; set; }
        public string MetricsJson { get; set; }

        public override string ToString()
        {
            return $"{CriterionName}: {Score}";
        }
    }

    // =========================================================
    // 16. COURSE DTO
    // =========================================================
    public class CourseDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Level { get; set; } // BEGINNER, INTERMEDIATE, ADVANCED
        public int PriceVND { get; set; }
        public bool IsPublished { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public List<LessonDTO> Lessons { get; set; }

        public CourseDTO()
        {
            Lessons = new List<LessonDTO>();
        }

        public string GetFormattedPrice()
        {
            return PriceVND.ToString("N0") + " VNĐ";
        }

        public string GetLevelText()
        {
            return Level switch
            {
                "BEGINNER" => "Sơ cấp",
                "INTERMEDIATE" => "Trung cấp",
                "ADVANCED" => "Nâng cao",
                _ => Level
            };
        }

        public override string ToString()
        {
            return $"{Title} ({GetLevelText()})";
        }
    }

    // =========================================================
    // 17. LESSON DTO
    // =========================================================
    public class LessonDTO
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoFilePath { get; set; }
        public int Position { get; set; }
        public bool IsPublished { get; set; }

        public bool HasVideo => !string.IsNullOrEmpty(VideoFilePath);

        public override string ToString()
        {
            return $"Bài {Position}: {Title}";
        }
    }

    // =========================================================
    // 18. ENROLLMENT DTO
    // =========================================================
    public class EnrollmentDTO
    {
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public string CourseTitle { get; set; }
        public DateTime EnrolledAt { get; set; }

        // Additional info
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }

        public decimal GetProgressPercentage()
        {
            if (TotalLessons == 0)
                return 0;
            return (decimal)CompletedLessons / TotalLessons * 100;
        }

        public override string ToString()
        {
            return $"{CourseTitle} - {GetProgressPercentage():F1}%";
        }
    }

    // =========================================================
    // 19. USER STATISTICS DTO
    // =========================================================
    public class UserStatisticsDTO
    {
        public long UserId { get; set; }
        public int TotalTests { get; set; }
        public decimal? BestBand { get; set; }
        public decimal? AverageListening { get; set; }
        public decimal? AverageReading { get; set; }
        public decimal? AverageWriting { get; set; }
        public decimal? AverageSpeaking { get; set; }
        public DateTime UpdatedAt { get; set; }

        public decimal? GetOverallAverage()
        {
            var scores = new List<decimal>();
            if (AverageListening.HasValue) scores.Add(AverageListening.Value);
            if (AverageReading.HasValue) scores.Add(AverageReading.Value);
            if (AverageWriting.HasValue) scores.Add(AverageWriting.Value);
            if (AverageSpeaking.HasValue) scores.Add(AverageSpeaking.Value);

            if (scores.Count == 0)
                return null;

            return scores.Average();
        }

        public string GetBestSkill()
        {
            var skills = new Dictionary<string, decimal?>
            {
                { "Listening", AverageListening },
                { "Reading", AverageReading },
                { "Writing", AverageWriting },
                { "Speaking", AverageSpeaking }
            };

            var best = skills.Where(s => s.Value.HasValue)
                           .OrderByDescending(s => s.Value)
                           .FirstOrDefault();

            return best.Key ?? "N/A";
        }

        public string GetWeakestSkill()
        {
            var skills = new Dictionary<string, decimal?>
            {
                { "Listening", AverageListening },
                { "Reading", AverageReading },
                { "Writing", AverageWriting },
                { "Speaking", AverageSpeaking }
            };

            var weakest = skills.Where(s => s.Value.HasValue)
                               .OrderBy(s => s.Value)
                               .FirstOrDefault();

            return weakest.Key ?? "N/A";
        }

        public override string ToString()
        {
            return $"Tests: {TotalTests}, Best: {BestBand?.ToString() ?? "N/A"}";
        }
    }

    // =========================================================
    // 20. ORDER DTO
    // =========================================================
    public class OrderDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int TotalVND { get; set; }
        public string Status { get; set; } // PENDING, PAID, CANCELLED
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }

        public bool IsPending => Status == "PENDING";
        public bool IsPaid => Status == "PAID";
        public bool IsCancelled => Status == "CANCELLED";

        public string GetFormattedTotal()
        {
            return TotalVND.ToString("N0") + " VNĐ";
        }

        public string GetStatusText()
        {
            return Status switch
            {
                "PENDING" => "Chờ thanh toán",
                "PAID" => "Đã thanh toán",
                "CANCELLED" => "Đã hủy",
                _ => Status
            };
        }

        public override string ToString()
        {
            return $"Order #{Id} - {GetFormattedTotal()} ({GetStatusText()})";
        }
    }

    // =========================================================
    // 21. LOGIN REQUEST DTO
    // =========================================================
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginRequestDTO() { }

        public LoginRequestDTO(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    // =========================================================
    // 22. REGISTER REQUEST DTO
    // =========================================================
    public class RegisterRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public RegisterRequestDTO() { }
    }

    // =========================================================
    // 23. TEST RESULT SUMMARY DTO (cho báo cáo)
    // =========================================================
    public class TestResultSummaryDTO
    {
        public long AttemptId { get; set; }
        public string PaperTitle { get; set; }
        public DateTime TestDate { get; set; }
        public decimal? OverallBand { get; set; }
        public decimal? ListeningBand { get; set; }
        public decimal? ReadingBand { get; set; }
        public decimal? WritingBand { get; set; }
        public decimal? SpeakingBand { get; set; }
        public string Duration { get; set; }

        public string GetOverallBandWithLabel()
        {
            if (!OverallBand.HasValue)
                return "Chưa chấm";

            return OverallBand.Value switch
            {
                >= 8.0m => $"{OverallBand} (Xuất sắc)",
                >= 7.0m => $"{OverallBand} (Tốt)",
                >= 6.0m => $"{OverallBand} (Khá)",
                >= 5.0m => $"{OverallBand} (Trung bình)",
                _ => $"{OverallBand} (Cần cải thiện)"
            };
        }

        public override string ToString()
        {
            return $"{PaperTitle} - Band {OverallBand?.ToString() ?? "N/A"}";
        }
    }

    // =========================================================
    // 24. LOGIN RESPONSE DTO (cho API Login)
    // =========================================================
    public class LoginResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDTO User { get; set; }

        public LoginResponseDTO()
        {
        }

        public LoginResponseDTO(bool success, string message, string token = null, UserDTO user = null)
        {
            Success = success;
            Message = message;
            Token = token;
            User = user;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static LoginResponseDTO FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LoginResponseDTO>(json);
        }
    }

    // =========================================================
    // 25. FORGOT PASSWORD REQUEST DTO
    // =========================================================
    public class ForgotPasswordRequestDTO
    {
        public string Email { get; set; }

        public ForgotPasswordRequestDTO() { }

        public ForgotPasswordRequestDTO(string email)
        {
            Email = email;
        }
    }

    // =========================================================
    // 26. FORGOT PASSWORD RESPONSE DTO
    // =========================================================
    public class ForgotPasswordResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string OtpToken { get; set; } // Token để verify OTP sau này

        public ForgotPasswordResponseDTO() { }

        public ForgotPasswordResponseDTO(bool success, string message, string otpToken = null)
        {
            Success = success;
            Message = message;
            OtpToken = otpToken;
        }
    }

    // =========================================================
    // 27. RESET PASSWORD REQUEST DTO
    // =========================================================
    public class ResetPasswordRequestDTO
    {
        public string Email { get; set; }
        public string OtpCode { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public ResetPasswordRequestDTO() { }
    }

}
