-- =========================================================
-- IELTS Learning System - SQL Server (Simplified Version)
-- 20 Tables - Core Features Only
-- =========================================================

USE master;
GO

IF DB_ID('IELTSLearning') IS NOT NULL
    DROP DATABASE IELTSLearning;
GO

CREATE DATABASE IELTSLearning;
GO

USE IELTSLearning;
GO

-- =========================================================
-- 1. USERS & AUTHENTICATION
-- =========================================================
CREATE TABLE Users (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    Email           NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(255) NOT NULL,
    FullName        NVARCHAR(255) NOT NULL,
    Phone           NVARCHAR(30),
    DateOfBirth     DATE,
    Role            NVARCHAR(20) NOT NULL DEFAULT 'STUDENT', -- STUDENT, TEACHER, ADMIN
    IsActive        BIT NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt       DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- 2. TEST PAPERS (Đề thi)
-- =========================================================
CREATE TABLE TestPapers (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    Code            NVARCHAR(50) UNIQUE,
    Title           NVARCHAR(255) NOT NULL,
    Description     NVARCHAR(MAX),
    IsPublished     BIT NOT NULL DEFAULT 0,
    CreatedBy       BIGINT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

ALTER TABLE TestPapers
ADD PdfFileName NVARCHAR(255),
    PdfFilePath NVARCHAR(500);
GO 

-- =========================================================
-- 3. TEST SECTIONS (Listening, Reading, Writing, Speaking)
-- =========================================================
CREATE TABLE TestSections (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    PaperId         BIGINT NOT NULL FOREIGN KEY REFERENCES TestPapers(Id),
    Skill           NVARCHAR(20) NOT NULL, -- LISTENING, READING, WRITING, SPEAKING
    TimeLimitMinutes INT,
    AudioFilePath   NVARCHAR(500), -- Cho Listening
    CONSTRAINT UQ_Section UNIQUE(PaperId, Skill)
);
GO

ALTER TABLE TestSections
ADD PdfFileName NVARCHAR(255),
    PdfFilePath NVARCHAR(500);
GO

-- =========================================================
-- 4. PASSAGES (Đoạn văn cho Reading/Listening)
-- =========================================================
CREATE TABLE Passages (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    SectionId       BIGINT NOT NULL FOREIGN KEY REFERENCES TestSections(Id),
    Title           NVARCHAR(255),
    ContentText     NVARCHAR(MAX),
    Position        INT NOT NULL,
    CONSTRAINT UQ_Passage UNIQUE(SectionId, Position)
);
GO

-- =========================================================
-- 5. QUESTIONS (Câu hỏi)
-- =========================================================
CREATE TABLE Questions (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    SectionId       BIGINT NOT NULL FOREIGN KEY REFERENCES TestSections(Id),
    PassageId       BIGINT FOREIGN KEY REFERENCES Passages(Id),
    QuestionType    NVARCHAR(30) NOT NULL, -- MCQ, FILL_BLANK, MATCHING, ESSAY, SPEAKING
    QuestionText    NVARCHAR(MAX) NOT NULL,
    Points          DECIMAL(5,2) NOT NULL DEFAULT 1.0,
    Position        INT NOT NULL,
    CONSTRAINT UQ_Question UNIQUE(SectionId, Position)
);
GO

-- =========================================================
-- 6. QUESTION CHOICES (Lựa chọn cho MCQ)
-- =========================================================
CREATE TABLE QuestionChoices (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    QuestionId      BIGINT NOT NULL FOREIGN KEY REFERENCES Questions(Id) ON DELETE CASCADE,
    ChoiceText      NVARCHAR(MAX) NOT NULL,
    IsCorrect       BIT NOT NULL DEFAULT 0,
    Position        INT NOT NULL
);
GO

-- =========================================================
-- 7. QUESTION ANSWER KEYS (Đáp án cho FILL_BLANK, MATCHING...)
-- =========================================================
CREATE TABLE QuestionAnswerKeys (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    QuestionId      BIGINT NOT NULL UNIQUE FOREIGN KEY REFERENCES Questions(Id) ON DELETE CASCADE,
    AnswerData      NVARCHAR(MAX) NOT NULL -- JSON format: {"key": "value"} hoặc ["ans1", "ans2"]
);
GO

-- =========================================================
-- 8. BAND SCALES (Thang điểm quy đổi)
-- =========================================================
CREATE TABLE BandScales (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    PaperId         BIGINT NOT NULL FOREIGN KEY REFERENCES TestPapers(Id),
    Skill           NVARCHAR(20) NOT NULL,
    RawScoreMin     INT NOT NULL,
    RawScoreMax     INT NOT NULL,
    BandScore       DECIMAL(3,1) NOT NULL,
    CONSTRAINT UQ_BandScale UNIQUE(PaperId, Skill, RawScoreMin, RawScoreMax)
);
GO

-- =========================================================
-- 9. TEST ATTEMPTS (Lượt thi)
-- =========================================================
CREATE TABLE TestAttempts (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    UserId          BIGINT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    PaperId         BIGINT NOT NULL FOREIGN KEY REFERENCES TestPapers(Id),
    AttemptNumber   INT NOT NULL,
    StartedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    FinishedAt      DATETIME2,
    Status          NVARCHAR(20) NOT NULL DEFAULT 'IN_PROGRESS', -- IN_PROGRESS, SUBMITTED, GRADED
    OverallBand     DECIMAL(3,1),
    CONSTRAINT UQ_Attempt UNIQUE(UserId, PaperId, AttemptNumber)
);
GO

-- =========================================================
-- 10. SECTION ATTEMPTS (Điểm từng phần)
-- =========================================================
CREATE TABLE SectionAttempts (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    TestAttemptId   BIGINT NOT NULL FOREIGN KEY REFERENCES TestAttempts(Id) ON DELETE CASCADE,
    SectionId       BIGINT NOT NULL FOREIGN KEY REFERENCES TestSections(Id),
    StartedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    FinishedAt      DATETIME2,
    RawScore        DECIMAL(6,2),
    BandScore       DECIMAL(3,1),
    Status          NVARCHAR(20) NOT NULL DEFAULT 'IN_PROGRESS',
    CONSTRAINT UQ_SectionAttempt UNIQUE(TestAttemptId, SectionId)
);
GO

-- =========================================================
-- 11. ANSWERS (Câu trả lời)
-- =========================================================
CREATE TABLE Answers (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    SectionAttemptId BIGINT NOT NULL FOREIGN KEY REFERENCES SectionAttempts(Id) ON DELETE CASCADE,
    QuestionId      BIGINT NOT NULL FOREIGN KEY REFERENCES Questions(Id),
    AnswerData      NVARCHAR(MAX), -- JSON: choice_id, text, mapping...
    IsCorrect       BIT,
    Score           DECIMAL(6,2),
    CONSTRAINT UQ_Answer UNIQUE(SectionAttemptId, QuestionId)
);
GO

-- =========================================================
-- 12. WRITING SUBMISSIONS (Bài viết)
-- =========================================================
CREATE TABLE WritingSubmissions (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    SectionAttemptId BIGINT NOT NULL UNIQUE FOREIGN KEY REFERENCES SectionAttempts(Id) ON DELETE CASCADE,
    PromptText      NVARCHAR(MAX),
    ContentText     NVARCHAR(MAX) NOT NULL,
    WordCount       INT,
    SubmittedAt     DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- 13. SPEAKING SUBMISSIONS (Bài nói - lưu đường dẫn file)
-- =========================================================
CREATE TABLE SpeakingSubmissions (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    SectionAttemptId BIGINT NOT NULL UNIQUE FOREIGN KEY REFERENCES SectionAttempts(Id) ON DELETE CASCADE,
    PromptText      NVARCHAR(MAX),
    AudioFilePath   NVARCHAR(500) NOT NULL,
    TranscriptText  NVARCHAR(MAX),
    DurationSeconds INT,
    SubmittedAt     DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- 14. AI EVALUATIONS (Chấm điểm AI cho Writing/Speaking)
-- =========================================================
CREATE TABLE AIEvaluations (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    SubmissionType  NVARCHAR(20) NOT NULL, -- WRITING, SPEAKING
    SubmissionId    BIGINT NOT NULL, -- FK động đến WritingSubmissions hoặc SpeakingSubmissions
    OverallScore    DECIMAL(6,2),
    BandScore       DECIMAL(3,1),
    FeedbackSummary NVARCHAR(MAX),
    DetailedFeedback NVARCHAR(MAX), -- JSON: {criterion: score, comment}
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- 15. COURSES (Khóa học)
-- =========================================================
CREATE TABLE Courses (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    Title           NVARCHAR(255) NOT NULL,
    Description     NVARCHAR(MAX),
    Level           NVARCHAR(30) NOT NULL, -- BEGINNER, INTERMEDIATE, ADVANCED
    PriceVND        INT NOT NULL DEFAULT 0,
    IsPublished     BIT NOT NULL DEFAULT 0,
    CreatedBy       BIGINT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- 16. LESSONS (Bài học trong khóa)
-- =========================================================
CREATE TABLE Lessons (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    CourseId        BIGINT NOT NULL FOREIGN KEY REFERENCES Courses(Id) ON DELETE CASCADE,
    Title           NVARCHAR(255) NOT NULL,
    Content         NVARCHAR(MAX),
    VideoFilePath   NVARCHAR(500),
    Position        INT NOT NULL,
    IsPublished     BIT NOT NULL DEFAULT 0,
    CONSTRAINT UQ_Lesson UNIQUE(CourseId, Position)
);
GO

-- =========================================================
-- 17. ENROLLMENTS (Đăng ký khóa học)
-- =========================================================
CREATE TABLE Enrollments (
    UserId          BIGINT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CourseId        BIGINT NOT NULL FOREIGN KEY REFERENCES Courses(Id),
    EnrolledAt      DATETIME2 NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (UserId, CourseId)
);
GO

-- =========================================================
-- 18. LESSON COMPLETIONS (Hoàn thành bài học)
-- =========================================================
CREATE TABLE LessonCompletions (
    UserId          BIGINT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    LessonId        BIGINT NOT NULL FOREIGN KEY REFERENCES Lessons(Id),
    CompletedAt     DATETIME2 NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (UserId, LessonId)
);
GO

-- =========================================================
-- 19. USER STATISTICS (Thống kê người dùng)
-- =========================================================
CREATE TABLE UserStatistics (
    UserId          BIGINT PRIMARY KEY FOREIGN KEY REFERENCES Users(Id),
    TotalTests      INT NOT NULL DEFAULT 0,
    BestBand        DECIMAL(3,1),
    AverageListening DECIMAL(3,1),
    AverageReading  DECIMAL(3,1),
    AverageWriting  DECIMAL(3,1),
    AverageSpeaking DECIMAL(3,1),
    UpdatedAt       DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- 20. ORDERS (Đơn hàng thanh toán khóa học)
-- =========================================================
CREATE TABLE Orders (
    Id              BIGINT PRIMARY KEY IDENTITY(1,1),
    UserId          BIGINT NOT NULL FOREIGN KEY REFERENCES Users(Id),
    CourseId        BIGINT NOT NULL FOREIGN KEY REFERENCES Courses(Id),
    TotalVND        INT NOT NULL,
    Status          NVARCHAR(20) NOT NULL DEFAULT 'PENDING', -- PENDING, PAID, CANCELLED
    CreatedAt       DATETIME2 NOT NULL DEFAULT GETDATE(),
    PaidAt          DATETIME2
);
GO

-- =========================================================
-- SEED DATA
-- =========================================================

-- Tạo tài khoản Admin mặc định
INSERT INTO Users (Email, PasswordHash, FullName, Role) 
VALUES ('buidinhhoang1910@gmail.com', '$2a$12$1LVVGxzbHkrUZL6r/zRmLukRMwUHD5DKhtZ13bAxSab2tKXgfDXlO', N'Administrator', 'ADMIN');

-- Tạo tài khoản Student demo
INSERT INTO Users (Email, PasswordHash, FullName, Role) 
VALUES ('student@test.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Nguyễn Văn A', 'STUDENT');

-- Tạo tài khoản Teacher demo
INSERT INTO Users (Email, PasswordHash, FullName, Role) 
VALUES ('teacher@ielts.com', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', N'Giáo viên B', 'TEACHER');

GO

PRINT 'Database created successfully with 20 core tables!';

select * from Users;
select * from TestPapers;

INSERT INTO TestPapers (Code, Title, Description, IsPublished, CreatedBy)
VALUES
('MT2025JAN', 'IELTS Mock Test 2025 January', 'Updated Cambridge style mock test for 2025', 1, 1),
('MT2024OCT', 'IELTS Mock Test 2024 October', 'Latest mock test 2024 Q4', 1, 1),
('MT2024JUL', 'IELTS Mock Test 2024 July', 'Mid-year edition mock test', 1, 1),
('MT2023DEC', 'IELTS Mock Test 2023 December', 'Updated end-year mock exam', 1, 1),
('MT2023APR', 'IELTS Mock Test 2023 April', 'Spring edition mock test', 1, 1),
('MT2022NOV', 'IELTS Mock Test 2022 November', 'Legacy Cambridge mock test', 1, 1),
('MT2022SEP', 'IELTS Mock Test 2022 September', 'Classic mock test 2022', 1, 1),
('MT2021JUN', 'IELTS Mock Test 2021 June', 'Old but gold mock test', 1, 1),
('MT2020FEB', 'IELTS Mock Test 2020 February', 'Starter mock test for beginners', 1, 1),
('MT2019OCT', 'IELTS Mock Test 2019 October', 'Archive version mock test', 1, 1);

INSERT INTO Courses (Title, Description, Level, PriceVND, IsPublished, CreatedBy)
VALUES
(N'IELTS Foundation', N'For beginners starting from band 0.0', 'BEGINNER', 1200000, 1, 1),
(N'IELTS 5.5–6.0 Booster', N'Improve to mid–intermediate band', 'INTERMEDIATE', 1800000, 1, 1),
(N'IELTS 6.0–7.5 Intensive', N'Advanced course targeting high bands', 'ADVANCED', 2400000, 1, 1),
(N'IELTS 7.5–9.0 Mastery', N'Elite band preparation for top scorers', 'ADVANCED', 3200000, 1, 1);

INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
VALUES
(1, 'LISTENING', 30, 'assets/audio/listening1.mp3', 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(1, 'READING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(1, 'WRITING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(1, 'SPEAKING', 15, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf');

INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
VALUES
(2, 'LISTENING', 30, 'assets/audio/listening2.mp3', 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(2, 'READING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(2, 'WRITING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(2, 'SPEAKING', 15, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf');

INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
VALUES
(3, 'LISTENING', 30, 'assets/audio/listening3.mp3', 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(3, 'READING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(3, 'WRITING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(3, 'SPEAKING', 15, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf');


INSERT INTO TestPapers (Code, Title, Description, IsPublished, CreatedBy)
VALUES
('MT2025JAN', 'IELTS Mock Test 2025 January', 'Updated Cambridge style mock test for 2025', 1, 1),
('MT2024OCT', 'IELTS Mock Test 2024 October', 'Latest mock test 2024 Q4', 1, 1),
('MT2024JUL', 'IELTS Mock Test 2024 July', 'Mid-year edition mock test', 1, 1),
('MT2023DEC', 'IELTS Mock Test 2023 December', 'Updated end-year mock exam', 1, 1),
('MT2023APR', 'IELTS Mock Test 2023 April', 'Spring edition mock test', 1, 1),
('MT2022NOV', 'IELTS Mock Test 2022 November', 'Legacy Cambridge mock test', 1, 1),
('MT2022SEP', 'IELTS Mock Test 2022 September', 'Classic mock test 2022', 1, 1),
('MT2021JUN', 'IELTS Mock Test 2021 June', 'Old but gold mock test', 1, 1),
('MT2020FEB', 'IELTS Mock Test 2020 February', 'Starter mock test for beginners', 1, 1),
('MT2019OCT', 'IELTS Mock Test 2019 October', 'Archive version mock test', 1, 1);

INSERT INTO Courses (Title, Description, Level, PriceVND, IsPublished, CreatedBy)
VALUES
(N'IELTS Foundation', N'For beginners starting from band 0.0', 'BEGINNER', 1200000, 1, 1),
(N'IELTS 5.5–6.0 Booster', N'Improve to mid–intermediate band', 'INTERMEDIATE', 1800000, 1, 1),
(N'IELTS 6.0–7.5 Intensive', N'Advanced course targeting high bands', 'ADVANCED', 2400000, 1, 1),
(N'IELTS 7.5–9.0 Mastery', N'Elite band preparation for top scorers', 'ADVANCED', 3200000, 1, 1);

INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
VALUES
(1, 'LISTENING', 30, 'assets/audio/listening1.mp3', 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(1, 'READING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(1, 'WRITING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(1, 'SPEAKING', 15, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf');

INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
VALUES
(2, 'LISTENING', 30, 'assets/audio/listening2.mp3', 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(2, 'READING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(2, 'WRITING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(2, 'SPEAKING', 15, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf');

INSERT INTO TestSections (PaperId, Skill, TimeLimitMinutes, AudioFilePath, PdfFileName, PdfFilePath)
VALUES
(3, 'LISTENING', 30, 'assets/audio/listening3.mp3', 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(3, 'READING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(3, 'WRITING', 60, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf'),
(3, 'SPEAKING', 15, NULL, 'C19-Test 4.pdf', 'assets/papers/C19-Test 4.pdf');


ALTER TABLE TestSections
ADD PdfFileName NVARCHAR(255),
    PdfFilePath NVARCHAR(500);
GO