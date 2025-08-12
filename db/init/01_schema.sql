-- =========================================================
-- Base settings
-- =========================================================
CREATE DATABASE IF NOT EXISTS english_learning
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE english_learning;

-- =========================================================
-- Identity & RBAC
-- =========================================================
CREATE TABLE users (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  email         VARCHAR(255) NOT NULL UNIQUE,
  password_hash VARCHAR(255) NOT NULL,
  full_name     VARCHAR(255) NOT NULL,
  avatar_url    VARCHAR(500),
  is_active     TINYINT(1) NOT NULL DEFAULT 1,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE roles (
  id        BIGINT PRIMARY KEY AUTO_INCREMENT,
  code      VARCHAR(50) NOT NULL UNIQUE,  -- 'STUDENT','STAFF','ADMIN'
  name      VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

CREATE TABLE user_roles (
  user_id BIGINT NOT NULL,
  role_id BIGINT NOT NULL,
  assigned_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id, role_id),
  CONSTRAINT fk_ur_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_ur_role FOREIGN KEY (role_id) REFERENCES roles(id)
) ENGINE=InnoDB;

-- =========================================================
-- Asset kho media (video/audio/pdf/image/subtitle, transcript)
-- =========================================================
CREATE TABLE assets (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  kind          ENUM('VIDEO','AUDIO','IMAGE','DOC','SUBTITLE','TRANSCRIPT','OTHER') NOT NULL,
  storage_url   VARCHAR(1000) NOT NULL,     -- s3/gcs/local path
  media_type    VARCHAR(100),               -- e.g. video/mp4
  duration_sec  INT,                        -- cho audio/video
  byte_size     BIGINT,
  sha256        CHAR(64),
  language_code VARCHAR(10),                -- en, vi, ...
  created_by    BIGINT NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_assets_user FOREIGN KEY (created_by) REFERENCES users(id),
  INDEX idx_assets_kind(kind),
  INDEX idx_assets_created_by(created_by)
) ENGINE=InnoDB;

-- =========================================================
-- Course structure: course -> module -> lesson (lecture video)
-- =========================================================
CREATE TABLE courses (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  title        VARCHAR(255) NOT NULL,
  description  TEXT,
  level        ENUM('BEGINNER','ELEMENTARY','PRE_INTERMEDIATE','INTERMEDIATE','UPPER_INTERMEDIATE','ADVANCED') NOT NULL DEFAULT 'BEGINNER',
  is_published TINYINT(1) NOT NULL DEFAULT 0,
  created_by   BIGINT NOT NULL,
  created_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_courses_user FOREIGN KEY (created_by) REFERENCES users(id)
) ENGINE=InnoDB;

CREATE TABLE modules (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  course_id   BIGINT NOT NULL,
  title       VARCHAR(255) NOT NULL,
  description TEXT,
  position    INT NOT NULL,
  created_at  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_modules_course FOREIGN KEY (course_id) REFERENCES courses(id),
  UNIQUE KEY uq_modules_course_pos (course_id, position)
) ENGINE=InnoDB;

CREATE TABLE lessons (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  module_id    BIGINT NOT NULL,
  title        VARCHAR(255) NOT NULL,
  objective    TEXT,
  video_id     BIGINT,                      -- assets.id (VIDEO)
  transcript_id BIGINT,                     -- assets.id (TRANSCRIPT)
  position     INT NOT NULL,
  is_published TINYINT(1) NOT NULL DEFAULT 0,
  created_by   BIGINT NOT NULL,
  created_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_lessons_module FOREIGN KEY (module_id) REFERENCES modules(id),
  CONSTRAINT fk_lessons_video FOREIGN KEY (video_id) REFERENCES assets(id),
  CONSTRAINT fk_lessons_transcript FOREIGN KEY (transcript_id) REFERENCES assets(id),
  CONSTRAINT fk_lessons_user FOREIGN KEY (created_by) REFERENCES users(id),
  UNIQUE KEY uq_lessons_module_pos (module_id, position)
) ENGINE=InnoDB;

-- =========================================================
-- Exercise & Question bank (đủ cho 4 skills)
-- =========================================================
CREATE TABLE exercises (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  title         VARCHAR(255) NOT NULL,
  skill         ENUM('READING','LISTENING','SPEAKING','WRITING') NOT NULL,
  description   TEXT,
  difficulty    ENUM('EASY','MEDIUM','HARD') DEFAULT 'MEDIUM',
  time_limit_sec INT,                        -- optional
  is_published  TINYINT(1) NOT NULL DEFAULT 0,
  created_by    BIGINT NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_ex_created_by FOREIGN KEY (created_by) REFERENCES users(id),
  INDEX idx_ex_skill(skill)
) ENGINE=InnoDB;

-- Gắn bài tập vào lesson (sau video). Có thứ tự.
CREATE TABLE lesson_exercises (
  lesson_id   BIGINT NOT NULL,
  exercise_id BIGINT NOT NULL,
  position    INT NOT NULL,
  PRIMARY KEY (lesson_id, exercise_id),
  CONSTRAINT fk_le_lesson FOREIGN KEY (lesson_id) REFERENCES lessons(id),
  CONSTRAINT fk_le_ex FOREIGN KEY (exercise_id) REFERENCES exercises(id),
  UNIQUE KEY uq_le_lesson_pos (lesson_id, position)
) ENGINE=InnoDB;

-- Passages cho Reading/Listening (đoạn văn/bài nghe)
CREATE TABLE passages (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  exercise_id  BIGINT NOT NULL,
  title        VARCHAR(255),
  content_text MEDIUMTEXT,                 -- reading text
  audio_id     BIGINT,                     -- listening audio (assets)
  transcript_id BIGINT,                    -- optional transcript (assets)
  created_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_pass_ex FOREIGN KEY (exercise_id) REFERENCES exercises(id),
  CONSTRAINT fk_pass_audio FOREIGN KEY (audio_id) REFERENCES assets(id),
  CONSTRAINT fk_pass_trans FOREIGN KEY (transcript_id) REFERENCES assets(id),
  UNIQUE KEY uq_pass_ex (exercise_id)
) ENGINE=InnoDB;

-- Question bank (nhiều loại)
CREATE TABLE questions (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  exercise_id   BIGINT NOT NULL,
  passage_id    BIGINT,                      -- null nếu không gắn passage
  qtype         ENUM('MCQ','MULTI_SELECT','FILL_BLANK','MATCHING','ORDERING','SHORT_ANSWER') NOT NULL,
  stem          TEXT NOT NULL,               -- đề
  meta_json     JSON,                        -- cấu hình phụ (ví dụ: số điểm, hint)
  position      INT NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_q_ex FOREIGN KEY (exercise_id) REFERENCES exercises(id),
  CONSTRAINT fk_q_pass FOREIGN KEY (passage_id) REFERENCES passages(id),
  UNIQUE KEY uq_q_ex_pos (exercise_id, position)
) ENGINE=InnoDB;

-- Choices (cho MCQ/MULTI_SELECT)
CREATE TABLE question_choices (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  question_id  BIGINT NOT NULL,
  content      TEXT NOT NULL,
  is_correct   TINYINT(1) NOT NULL DEFAULT 0,
  position     INT NOT NULL,
  CONSTRAINT fk_choice_q FOREIGN KEY (question_id) REFERENCES questions(id),
  UNIQUE KEY uq_choice_q_pos (question_id, position),
  INDEX idx_choice_q (question_id)
) ENGINE=InnoDB;

-- Answer keys cho các loại khác (fill/matching/ordering/short answer)
CREATE TABLE question_answer_keys (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  question_id  BIGINT NOT NULL,
  key_json     JSON NOT NULL,  -- ví dụ: {"blanks":["cat","dogs"]} hoặc mapping cho matching/ordering, regex cho short answer
  CONSTRAINT fk_key_q FOREIGN KEY (question_id) REFERENCES questions(id),
  UNIQUE KEY uq_key_q (question_id)
) ENGINE=InnoDB;

-- =========================================================
-- Enrollments & progress
-- =========================================================
CREATE TABLE enrollments (
  user_id   BIGINT NOT NULL,
  course_id BIGINT NOT NULL,
  enrolled_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id, course_id),
  CONSTRAINT fk_en_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_en_course FOREIGN KEY (course_id) REFERENCES courses(id)
) ENGINE=InnoDB;

CREATE TABLE lesson_completions (
  user_id   BIGINT NOT NULL,
  lesson_id BIGINT NOT NULL,
  completed_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id, lesson_id),
  CONSTRAINT fk_lc_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_lc_lesson FOREIGN KEY (lesson_id) REFERENCES lessons(id)
) ENGINE=InnoDB;

-- =========================================================
-- Submissions (common wrapper) & objective grading
-- =========================================================
CREATE TABLE exercise_attempts (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id       BIGINT NOT NULL,
  exercise_id   BIGINT NOT NULL,
  attempt_no    INT NOT NULL,                       -- lần thứ mấy
  started_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  finished_at   DATETIME,
  raw_score     DECIMAL(6,2),                       -- điểm objective (reading/listening) trước scale
  scaled_score  DECIMAL(6,2),
  max_score     DECIMAL(6,2),
  status        ENUM('IN_PROGRESS','SUBMITTED','GRADED','CANCELLED') NOT NULL DEFAULT 'IN_PROGRESS',
  meta_json     JSON,
  CONSTRAINT fk_att_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_att_ex FOREIGN KEY (exercise_id) REFERENCES exercises(id),
  UNIQUE KEY uq_att_user_ex_no (user_id, exercise_id, attempt_no),
  INDEX idx_att_ex (exercise_id),
  INDEX idx_att_user (user_id)
) ENGINE=InnoDB;

-- Lưu câu trả lời cho từng câu hỏi (objective)
CREATE TABLE attempt_answers (
  id              BIGINT PRIMARY KEY AUTO_INCREMENT,
  attempt_id      BIGINT NOT NULL,
  question_id     BIGINT NOT NULL,
  answer_json     JSON NOT NULL,      -- lựa chọn, text điền, mapping,...
  is_correct      TINYINT(1),
  earned_score    DECIMAL(6,2),
  checked_at      DATETIME,
  CONSTRAINT fk_ans_att FOREIGN KEY (attempt_id) REFERENCES exercise_attempts(id),
  CONSTRAINT fk_ans_q FOREIGN KEY (question_id) REFERENCES questions(id),
  UNIQUE KEY uq_ans_att_q (attempt_id, question_id)
) ENGINE=InnoDB;

-- =========================================================
-- Speaking/Writing submissions & AI grading
-- =========================================================
CREATE TABLE ai_models (
  id             BIGINT PRIMARY KEY AUTO_INCREMENT,
  provider       VARCHAR(100) NOT NULL,   -- 'OpenAI','Azure','Local'
  model_name     VARCHAR(100) NOT NULL,   -- 'gpt-4o-mini', 'whisper-large-v3', ...
  version_tag    VARCHAR(50),             -- '2025-07-01'
  purpose        ENUM('ASR','LLM_SCORING','PRONUNCIATION','GRAMMAR') NOT NULL,
  config_json    JSON,
  UNIQUE KEY uq_model (provider, model_name, COALESCE(version_tag,'')),
  INDEX idx_model_purpose (purpose)
) ENGINE=InnoDB;

-- Speaking: nộp file audio, có transcript (ASR)
CREATE TABLE speaking_submissions (
  id               BIGINT PRIMARY KEY AUTO_INCREMENT,
  attempt_id       BIGINT NOT NULL,             -- exercise_attempts.id (exercise skill=SPEAKING)
  prompt_text      TEXT,
  audio_asset_id   BIGINT NOT NULL,             -- assets.id (AUDIO)
  asr_text         MEDIUMTEXT,                  -- transcript từ ASR
  words_count      INT,
  duration_sec     INT,
  created_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_spk_att FOREIGN KEY (attempt_id) REFERENCES exercise_attempts(id),
  CONSTRAINT fk_spk_audio FOREIGN KEY (audio_asset_id) REFERENCES assets(id),
  UNIQUE KEY uq_spk_att (attempt_id)
) ENGINE=InnoDB;

-- Writing: nộp văn bản
CREATE TABLE writing_submissions (
  id               BIGINT PRIMARY KEY AUTO_INCREMENT,
  attempt_id       BIGINT NOT NULL,             -- exercise_attempts.id (exercise skill=WRITING)
  prompt_text      TEXT,
  content_text     MEDIUMTEXT NOT NULL,
  words_count      INT,
  created_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_wri_att FOREIGN KEY (attempt_id) REFERENCES exercise_attempts(id),
  UNIQUE KEY uq_wri_att (attempt_id)
) ENGINE=InnoDB;

-- Rubric chung (để tái sử dụng và versioning)
CREATE TABLE rubrics (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  name         VARCHAR(255) NOT NULL,           -- IELTS Speaking 4-band, IELTS Writing Task 2, ...
  skill        ENUM('SPEAKING','WRITING') NOT NULL,
  version_tag  VARCHAR(50) NOT NULL,
  description  TEXT,
  UNIQUE KEY uq_rubric (name, version_tag, skill)
) ENGINE=InnoDB;

CREATE TABLE rubric_criteria (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  rubric_id   BIGINT NOT NULL,
  code        VARCHAR(100) NOT NULL,            -- 'FLUENCY','PRONUNCIATION','LEXICAL','GRAMMAR'...
  display_name VARCHAR(255) NOT NULL,
  weight      DECIMAL(4,2) NOT NULL DEFAULT 1.0,
  description TEXT,
  CONSTRAINT fk_rc_rubric FOREIGN KEY (rubric_id) REFERENCES rubrics(id),
  UNIQUE KEY uq_rc (rubric_id, code)
) ENGINE=InnoDB;

-- Kết quả chấm AI (phân tách theo submission để lưu lịch sử và model)
CREATE TABLE ai_evaluations (
  id               BIGINT PRIMARY KEY AUTO_INCREMENT,
  submission_kind  ENUM('SPEAKING','WRITING') NOT NULL,
  submission_id    BIGINT NOT NULL,      -- speaking_submissions.id hoặc writing_submissions.id
  rubric_id        BIGINT NOT NULL,
  model_id         BIGINT NOT NULL,
  overall_score    DECIMAL(6,2),
  band_score       DECIMAL(4,1),         -- ví dụ IELTS 6.5
  feedback_summary MEDIUMTEXT,           -- tổng nhận xét
  raw_response_json JSON,                -- nguyên phản hồi từ model (audit/debug)
  created_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_eval_rubric FOREIGN KEY (rubric_id) REFERENCES rubrics(id),
  CONSTRAINT fk_eval_model FOREIGN KEY (model_id) REFERENCES ai_models(id),
  INDEX idx_eval_kind_sub (submission_kind, submission_id),
  INDEX idx_eval_created (created_at)
) ENGINE=InnoDB;

CREATE TABLE ai_evaluation_details (
  id             BIGINT PRIMARY KEY AUTO_INCREMENT,
  evaluation_id  BIGINT NOT NULL,
  criterion_id   BIGINT NOT NULL,       -- rubric_criteria.id
  score          DECIMAL(6,2) NOT NULL,
  feedback_text  TEXT,
  metrics_json   JSON,                  -- chi tiết: WPM, pause rate, WER, grammar errors,...
  CONSTRAINT fk_ed_eval FOREIGN KEY (evaluation_id) REFERENCES ai_evaluations(id),
  CONSTRAINT fk_ed_crit FOREIGN KEY (criterion_id) REFERENCES rubric_criteria(id),
  UNIQUE KEY uq_ed (evaluation_id, criterion_id)
) ENGINE=InnoDB;

-- Link tổng hợp điểm về attempt (sau khi có AI/objective)
-- Có thể tạo VIEW để lấy điểm cuối cùng
CREATE VIEW v_attempt_final_score AS
SELECT a.id AS attempt_id,
       a.user_id,
       a.exercise_id,
       a.raw_score,
       a.scaled_score,
       a.max_score,
       a.status
FROM exercise_attempts a;

-- =========================================================
-- Content governance & audit
-- =========================================================
CREATE TABLE content_reviews (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  entity_kind  ENUM('COURSE','MODULE','LESSON','EXERCISE') NOT NULL,
  entity_id    BIGINT NOT NULL,
  reviewer_id  BIGINT NOT NULL,
  status       ENUM('PENDING','APPROVED','REJECTED') NOT NULL DEFAULT 'PENDING',
  comment      TEXT,
  reviewed_at  DATETIME,
  created_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_cr_reviewer FOREIGN KEY (reviewer_id) REFERENCES users(id),
  INDEX idx_cr_entity (entity_kind, entity_id)
) ENGINE=InnoDB;

CREATE TABLE audit_logs (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id     BIGINT,
  action      VARCHAR(100) NOT NULL,     -- CREATE_EXERCISE, PUBLISH_LESSON, GRADE_AI,...
  entity_kind VARCHAR(50),
  entity_id   BIGINT,
  data_json   JSON,
  created_at  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_audit_user (user_id),
  INDEX idx_audit_entity (entity_kind, entity_id)
) ENGINE=InnoDB;

-- Seed roles
INSERT IGNORE INTO roles(code, name) VALUES
('STUDENT','Học viên'),
('STAFF','Nhân viên'),
('ADMIN','Quản trị viên');
