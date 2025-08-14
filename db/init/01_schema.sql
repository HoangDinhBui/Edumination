-- =========================================================
-- Base
-- =========================================================
CREATE DATABASE IF NOT EXISTS english_learning
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE english_learning;

-- =========================================================
-- Identity, OAuth2 & RBAC
-- =========================================================
CREATE TABLE users (
  id              BIGINT PRIMARY KEY AUTO_INCREMENT,
  email           VARCHAR(255) NOT NULL UNIQUE,
  email_verified  TINYINT(1) NOT NULL DEFAULT 0,
  password_hash   VARCHAR(255),                         -- NULL nếu chỉ đăng nhập OAuth
  full_name       VARCHAR(255) NOT NULL,
  avatar_url      VARCHAR(500),
  is_active       TINYINT(1) NOT NULL DEFAULT 1,
  created_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  updated_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE roles (
  id    BIGINT PRIMARY KEY AUTO_INCREMENT,
  code  VARCHAR(50) NOT NULL UNIQUE,                   -- 'STUDENT','TEACHER','ADMIN'
  name  VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

CREATE TABLE user_roles (
  user_id   BIGINT NOT NULL,
  role_id   BIGINT NOT NULL,
  assigned_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id, role_id),
  CONSTRAINT fk_ur_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_ur_role FOREIGN KEY (role_id) REFERENCES roles(id)
) ENGINE=InnoDB;

-- Thông tin hồ sơ cần bổ sung lần đầu khi login OAuth
CREATE TABLE user_profiles (
  user_id        BIGINT PRIMARY KEY,
  dob            DATE NULL,
  phone          VARCHAR(30) NULL,
  country        VARCHAR(100) NULL,
  city           VARCHAR(100) NULL,
  english_goal   VARCHAR(255) NULL,                    -- mục tiêu học
  confirmed_at   DATETIME NULL,                        -- người dùng xác nhận hồ sơ (first-time confirm)
  updated_at     DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_profiles_user FOREIGN KEY (user_id) REFERENCES users(id)
) ENGINE=InnoDB;

-- Nhà cung cấp OAuth & liên kết tài khoản
CREATE TABLE oauth_providers (
  id        BIGINT PRIMARY KEY AUTO_INCREMENT,
  code      VARCHAR(50) NOT NULL UNIQUE,              -- 'GOOGLE','FACEBOOK','GITHUB',...
  name      VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

CREATE TABLE oauth_accounts (
  id              BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id         BIGINT NOT NULL,
  provider_id     BIGINT NOT NULL,
  provider_uid    VARCHAR(255) NOT NULL,              -- sub/ id từ nhà cung cấp
  email_at_provider VARCHAR(255),
  access_token    TEXT,
  refresh_token   TEXT,
  linked_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_oauth (provider_id, provider_uid),
  CONSTRAINT fk_oa_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_oa_provider FOREIGN KEY (provider_id) REFERENCES oauth_providers(id)
) ENGINE=InnoDB;

-- Xác minh email, reset mật khẩu
CREATE TABLE email_verifications (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id     BIGINT NOT NULL,
  token_hash  CHAR(64) NOT NULL,
  expires_at  DATETIME NOT NULL,
  used_at     DATETIME NULL,
  created_at  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_ev_user (user_id),
  CONSTRAINT fk_ev_user FOREIGN KEY (user_id) REFERENCES users(id)
) ENGINE=InnoDB;

CREATE TABLE password_resets (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id     BIGINT NOT NULL,
  token_hash  CHAR(64) NOT NULL,
  expires_at  DATETIME NOT NULL,
  used_at     DATETIME NULL,
  created_at  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_pr_user (user_id),
  CONSTRAINT fk_pr_user FOREIGN KEY (user_id) REFERENCES users(id)
) ENGINE=InnoDB;

-- Domain EDU (gmail trường) để lọc leaderboard
CREATE TABLE edu_domains (
  id        BIGINT PRIMARY KEY AUTO_INCREMENT,
  domain    VARCHAR(255) NOT NULL UNIQUE               -- ví dụ: 'student.abc.edu.vn'
) ENGINE=InnoDB;

-- =========================================================
-- Media kho (PDF/Audio/Images/Docs)
-- =========================================================
CREATE TABLE assets (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  kind          ENUM('VIDEO','AUDIO','IMAGE','DOC','PDF','SUBTITLE','TRANSCRIPT','OTHER') NOT NULL,
  storage_url   VARCHAR(1000) NOT NULL,               -- s3/gcs/local
  media_type    VARCHAR(100),                          -- 'application/pdf','audio/mp3',...
  duration_sec  INT,
  byte_size     BIGINT,
  sha256        CHAR(64),
  language_code VARCHAR(10),                           -- 'en','vi',...
  created_by    BIGINT NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_assets_user FOREIGN KEY (created_by) REFERENCES users(id),
  INDEX idx_assets_kind(kind),
  INDEX idx_assets_creator(created_by)
) ENGINE=InnoDB;

-- =========================================================
-- IELTS Test Library (Paper → Sections → Passages/Questions)
-- =========================================================
CREATE TABLE test_papers (
  id              BIGINT PRIMARY KEY AUTO_INCREMENT,
  code            VARCHAR(100) UNIQUE,                 -- mã paper (nếu có)
  title           VARCHAR(255) NOT NULL,
  source_type     ENUM('OFFICIAL','CUSTOM') NOT NULL DEFAULT 'CUSTOM',
  upload_method   ENUM('PDF_PARSER','MANUAL') NOT NULL,  -- cách Giáo viên up
  pdf_asset_id    BIGINT NULL,                         -- file PDF gốc nếu có
  status          ENUM('DRAFT','REVIEW','PUBLISHED','ARCHIVED') NOT NULL DEFAULT 'DRAFT',
  created_by      BIGINT NOT NULL,
  created_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  published_at    DATETIME NULL,
  CONSTRAINT fk_tp_pdf FOREIGN KEY (pdf_asset_id) REFERENCES assets(id),
  CONSTRAINT fk_tp_creator FOREIGN KEY (created_by) REFERENCES users(id)
) ENGINE=InnoDB;

CREATE TABLE test_sections (
  id              BIGINT PRIMARY KEY AUTO_INCREMENT,
  paper_id        BIGINT NOT NULL,
  skill           ENUM('LISTENING','READING','WRITING','SPEAKING') NOT NULL,
  section_no      TINYINT NOT NULL,                    -- 1..4
  time_limit_sec  INT,                                  -- thời lượng từng phần
  audio_asset_id  BIGINT NULL,                          -- cho Listening nếu có
  is_published    TINYINT(1) NOT NULL DEFAULT 0,
  UNIQUE KEY uq_ts (paper_id, skill),
  CONSTRAINT fk_ts_paper FOREIGN KEY (paper_id) REFERENCES test_papers(id),
  CONSTRAINT fk_ts_audio FOREIGN KEY (audio_asset_id) REFERENCES assets(id)
) ENGINE=InnoDB;

-- Đoạn bài đọc/nghe theo Section (0..n)
CREATE TABLE passages (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  section_id    BIGINT NOT NULL,
  title         VARCHAR(255),
  content_text  MEDIUMTEXT,                             -- nội dung Reading
  audio_id      BIGINT,                                 -- sub-audio (nếu Listening có nhiều file)
  transcript_id BIGINT,
  position      INT NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_pass_section FOREIGN KEY (section_id) REFERENCES test_sections(id),
  CONSTRAINT fk_pass_audio FOREIGN KEY (audio_id) REFERENCES assets(id),
  CONSTRAINT fk_pass_trans FOREIGN KEY (transcript_id) REFERENCES assets(id),
  UNIQUE KEY uq_pass_section_pos (section_id, position)
) ENGINE=InnoDB;

-- Ngân hàng câu hỏi theo Section (gắn Passage nếu cần)
CREATE TABLE questions (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  section_id    BIGINT NOT NULL,
  passage_id    BIGINT NULL,
  qtype         ENUM('MCQ','MULTI_SELECT','FILL_BLANK','MATCHING','ORDERING','SHORT_ANSWER','ESSAY','SPEAK_PROMPT') NOT NULL,
  stem          MEDIUMTEXT NOT NULL,
  meta_json     JSON,                                   -- điểm, hint, rubric nhỏ, số blank...
  position      INT NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_q_section FOREIGN KEY (section_id) REFERENCES test_sections(id),
  CONSTRAINT fk_q_passage FOREIGN KEY (passage_id) REFERENCES passages(id),
  UNIQUE KEY uq_q_section_pos (section_id, position)
) ENGINE=InnoDB;

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

CREATE TABLE question_answer_keys (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  question_id  BIGINT NOT NULL,
  key_json     JSON NOT NULL,                            -- keys cho fill/matching/ordering/regex...
  CONSTRAINT fk_key_q FOREIGN KEY (question_id) REFERENCES questions(id),
  UNIQUE KEY uq_key_q (question_id)
) ENGINE=InnoDB;

-- Thang quy đổi raw->band từng kỹ năng (để scale theo đề/version)
CREATE TABLE band_scales (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  paper_id    BIGINT NOT NULL,
  skill       ENUM('LISTENING','READING','WRITING','SPEAKING') NOT NULL,
  raw_min     INT NOT NULL,
  raw_max     INT NOT NULL,
  band        DECIMAL(3,1) NOT NULL,                    -- ví dụ 6.5
  UNIQUE KEY uq_scale (paper_id, skill, raw_min, raw_max),
  CONSTRAINT fk_scale_paper FOREIGN KEY (paper_id) REFERENCES test_papers(id)
) ENGINE=InnoDB;

-- =========================================================
-- Làm bài & chấm điểm (test attempts)
-- =========================================================
CREATE TABLE test_attempts (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id       BIGINT NOT NULL,
  paper_id      BIGINT NOT NULL,
  attempt_no    INT NOT NULL,
  started_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  finished_at   DATETIME NULL,
  status        ENUM('IN_PROGRESS','SUBMITTED','GRADED','CANCELLED') NOT NULL DEFAULT 'IN_PROGRESS',
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY uq_attempt (user_id, paper_id, attempt_no),
  INDEX idx_ta_user (user_id),
  INDEX idx_ta_paper (paper_id),
  CONSTRAINT fk_ta_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_ta_paper FOREIGN KEY (paper_id) REFERENCES test_papers(id)
) ENGINE=InnoDB;

CREATE TABLE section_attempts (
  id              BIGINT PRIMARY KEY AUTO_INCREMENT,
  test_attempt_id BIGINT NOT NULL,
  section_id      BIGINT NOT NULL,
  started_at      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  finished_at     DATETIME NULL,
  raw_score       DECIMAL(6,2) NULL,                    -- objective (L/R); W/S tính điểm ở bảng AI
  scaled_band     DECIMAL(3,1) NULL,                    -- quy đổi theo band_scales
  status          ENUM('IN_PROGRESS','SUBMITTED','GRADED','CANCELLED') NOT NULL DEFAULT 'IN_PROGRESS',
  UNIQUE KEY uq_sa (test_attempt_id, section_id),
  INDEX idx_sa_section (section_id),
  CONSTRAINT fk_sa_ta FOREIGN KEY (test_attempt_id) REFERENCES test_attempts(id),
  CONSTRAINT fk_sa_section FOREIGN KEY (section_id) REFERENCES test_sections(id)
) ENGINE=InnoDB;

CREATE TABLE answers (
  id              BIGINT PRIMARY KEY AUTO_INCREMENT,
  section_attempt_id BIGINT NOT NULL,
  question_id     BIGINT NOT NULL,
  answer_json     JSON NOT NULL,                        -- lựa chọn/text/mapping...
  is_correct      TINYINT(1) NULL,                      -- cho objective
  earned_score    DECIMAL(6,2) NULL,
  checked_at      DATETIME NULL,
  UNIQUE KEY uq_ans (section_attempt_id, question_id),
  CONSTRAINT fk_ans_sa FOREIGN KEY (section_attempt_id) REFERENCES section_attempts(id),
  CONSTRAINT fk_ans_q FOREIGN KEY (question_id) REFERENCES questions(id)
) ENGINE=InnoDB;

-- Submissions cho Speaking/Writing
CREATE TABLE speaking_submissions (
  id               BIGINT PRIMARY KEY AUTO_INCREMENT,
  section_attempt_id BIGINT NOT NULL,
  prompt_text      TEXT,
  audio_asset_id   BIGINT NOT NULL,
  asr_text         MEDIUMTEXT,
  words_count      INT,
  duration_sec     INT,
  created_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_spk_sa FOREIGN KEY (section_attempt_id) REFERENCES section_attempts(id),
  CONSTRAINT fk_spk_audio FOREIGN KEY (audio_asset_id) REFERENCES assets(id),
  UNIQUE KEY uq_spk_sa (section_attempt_id)
) ENGINE=InnoDB;

CREATE TABLE writing_submissions (
  id               BIGINT PRIMARY KEY AUTO_INCREMENT,
  section_attempt_id BIGINT NOT NULL,
  prompt_text      TEXT,
  content_text     MEDIUMTEXT NOT NULL,
  words_count      INT,
  created_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_wri_sa FOREIGN KEY (section_attempt_id) REFERENCES section_attempts(id),
  UNIQUE KEY uq_wri_sa (section_attempt_id)
) ENGINE=InnoDB;

-- AI model & chấm điểm (W/S)
CREATE TABLE ai_models (
  id             BIGINT PRIMARY KEY AUTO_INCREMENT,
  provider       VARCHAR(100) NOT NULL,                 -- 'OpenAI','Azure','Local'
  model_name     VARCHAR(100) NOT NULL,                 -- 'gpt-4o-mini', 'whisper-large', ...
  version_tag    VARCHAR(50),
  purpose        ENUM('ASR','LLM_SCORING','PRONUNCIATION','GRAMMAR') NOT NULL,
  config_json    JSON,
  UNIQUE KEY uq_model (provider, model_name, COALESCE(version_tag,'')),
  INDEX idx_model_purpose (purpose)
) ENGINE=InnoDB;

CREATE TABLE rubrics (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  name         VARCHAR(255) NOT NULL,                   -- 'IELTS Speaking', 'IELTS Writing Task 2'
  skill        ENUM('SPEAKING','WRITING') NOT NULL,
  version_tag  VARCHAR(50) NOT NULL,
  description  TEXT,
  UNIQUE KEY uq_rubric (name, version_tag, skill)
) ENGINE=InnoDB;

CREATE TABLE rubric_criteria (
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  rubric_id     BIGINT NOT NULL,
  code          VARCHAR(100) NOT NULL,                  -- 'FLUENCY','LEXICAL','COHERENCE','GRAMMAR','PRON'
  display_name  VARCHAR(255) NOT NULL,
  weight        DECIMAL(4,2) NOT NULL DEFAULT 1.0,
  description   TEXT,
  CONSTRAINT fk_rc_rubric FOREIGN KEY (rubric_id) REFERENCES rubrics(id),
  UNIQUE KEY uq_rc (rubric_id, code)
) ENGINE=InnoDB;

CREATE TABLE ai_evaluations (
  id               BIGINT PRIMARY KEY AUTO_INCREMENT,
  submission_kind  ENUM('SPEAKING','WRITING') NOT NULL,
  submission_id    BIGINT NOT NULL,                     -- FK động: tới speaking_submissions.id hoặc writing_submissions.id
  rubric_id        BIGINT NOT NULL,
  model_id         BIGINT NOT NULL,
  overall_score    DECIMAL(6,2) NULL,
  band_score       DECIMAL(3,1) NULL,
  feedback_summary MEDIUMTEXT,
  raw_response_json JSON,
  created_at       DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_eval_rubric FOREIGN KEY (rubric_id) REFERENCES rubrics(id),
  CONSTRAINT fk_eval_model FOREIGN KEY (model_id) REFERENCES ai_models(id),
  INDEX idx_eval_kind_sub (submission_kind, submission_id),
  INDEX idx_eval_created (created_at)
) ENGINE=InnoDB;

CREATE TABLE ai_evaluation_details (
  id             BIGINT PRIMARY KEY AUTO_INCREMENT,
  evaluation_id  BIGINT NOT NULL,
  criterion_id   BIGINT NOT NULL,
  score          DECIMAL(6,2) NOT NULL,
  feedback_text  TEXT,
  metrics_json   JSON,
  CONSTRAINT fk_ed_eval FOREIGN KEY (evaluation_id) REFERENCES ai_evaluations(id),
  CONSTRAINT fk_ed_crit FOREIGN KEY (criterion_id) REFERENCES rubric_criteria(id),
  UNIQUE KEY uq_ed (evaluation_id, criterion_id)
) ENGINE=InnoDB;

-- View tổng điểm một lần thi (4 kỹ năng -> overall band ~ trung bình .5 step)
CREATE VIEW v_test_attempt_band AS
SELECT 
  ta.id AS test_attempt_id,
  ta.user_id,
  ta.paper_id,
  ROUND(
    (AVG(CASE WHEN ts.skill='LISTENING' THEN sa.scaled_band END) +
     AVG(CASE WHEN ts.skill='READING'   THEN sa.scaled_band END) +
     AVG(CASE WHEN ts.skill='WRITING'   THEN sa.scaled_band END) +
     AVG(CASE WHEN ts.skill='SPEAKING'  THEN sa.scaled_band END)
    ), 1
  ) AS overall_band
FROM test_attempts ta
JOIN section_attempts sa ON sa.test_attempt_id = ta.id
JOIN test_sections ts ON ts.id = sa.section_id
GROUP BY ta.id, ta.user_id, ta.paper_id;

-- =========================================================
-- Khóa học & Gợi ý theo band
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

-- Quy tắc band để recommend khóa học
CREATE TABLE course_band_rules (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  course_id   BIGINT NOT NULL,
  band_min    DECIMAL(3,1) NOT NULL,                   -- ví dụ: 0.0
  band_max    DECIMAL(3,1) NOT NULL,                   -- ví dụ: 6.0 (bao gồm)
  UNIQUE KEY uq_cbr (course_id, band_min, band_max),
  CONSTRAINT fk_cbr_course FOREIGN KEY (course_id) REFERENCES courses(id)
) ENGINE=InnoDB;

-- Cấu trúc module/lesson (nếu triển khai bài giảng video)
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
  id            BIGINT PRIMARY KEY AUTO_INCREMENT,
  module_id     BIGINT NOT NULL,
  title         VARCHAR(255) NOT NULL,
  objective     TEXT,
  video_id      BIGINT,
  transcript_id BIGINT,
  position      INT NOT NULL,
  is_published  TINYINT(1) NOT NULL DEFAULT 0,
  created_by    BIGINT NOT NULL,
  created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_lessons_module FOREIGN KEY (module_id) REFERENCES modules(id),
  CONSTRAINT fk_lessons_video FOREIGN KEY (video_id) REFERENCES assets(id),
  CONSTRAINT fk_lessons_trans FOREIGN KEY (transcript_id) REFERENCES assets(id),
  CONSTRAINT fk_lessons_user FOREIGN KEY (created_by) REFERENCES users(id),
  UNIQUE KEY uq_lessons_module_pos (module_id, position)
) ENGINE=InnoDB;

CREATE TABLE enrollments (
  user_id     BIGINT NOT NULL,
  course_id   BIGINT NOT NULL,
  enrolled_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id, course_id),
  CONSTRAINT fk_en_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_en_course FOREIGN KEY (course_id) REFERENCES courses(id)
) ENGINE=InnoDB;

CREATE TABLE lesson_completions (
  user_id     BIGINT NOT NULL,
  lesson_id   BIGINT NOT NULL,
  completed_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (user_id, lesson_id),
  CONSTRAINT fk_lc_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_lc_lesson FOREIGN KEY (lesson_id) REFERENCES lessons(id)
) ENGINE=InnoDB;

-- =========================================================
-- Leaderboard (EDU only) & Dashboard stats
-- =========================================================
-- View lọc user có domain EDU
CREATE VIEW v_users_edu AS
SELECT u.*
FROM users u
JOIN (
  SELECT DISTINCT SUBSTRING_INDEX(email, '@', -1) AS domain, id
  FROM users
) edumap ON edumap.id = u.id
JOIN edu_domains d ON d.domain = SUBSTRING_INDEX(u.email, '@', -1);

-- Bảng điểm “best-attempt” cho xếp hạng (denormalize để truy vấn nhanh)
CREATE TABLE leaderboard_entries (
  id             BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id        BIGINT NOT NULL,
  paper_id       BIGINT NOT NULL,
  best_overall_band DECIMAL(3,1) NOT NULL,
  best_at        DATETIME NOT NULL,
  UNIQUE KEY uq_lb (user_id, paper_id),
  INDEX idx_lb_band (best_overall_band),
  CONSTRAINT fk_lb_user FOREIGN KEY (user_id) REFERENCES users(id),
  CONSTRAINT fk_lb_paper FOREIGN KEY (paper_id) REFERENCES test_papers(id)
) ENGINE=InnoDB;

-- Số liệu dashboard (cập nhật bằng job/trigger)
CREATE TABLE user_stats (
  user_id             BIGINT PRIMARY KEY,
  total_tests         INT NOT NULL DEFAULT 0,
  best_band           DECIMAL(3,1) NULL,
  worst_band          DECIMAL(3,1) NULL,
  best_skill          ENUM('LISTENING','READING','WRITING','SPEAKING') NULL,
  worst_skill         ENUM('LISTENING','READING','WRITING','SPEAKING') NULL,
  avg_listening_band  DECIMAL(3,1) NULL,
  avg_reading_band    DECIMAL(3,1) NULL,
  avg_writing_band    DECIMAL(3,1) NULL,
  avg_speaking_band   DECIMAL(3,1) NULL,
  updated_at          DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  CONSTRAINT fk_us_user FOREIGN KEY (user_id) REFERENCES users(id)
) ENGINE=InnoDB;

-- =========================================================
-- Content review & Audit
-- =========================================================
CREATE TABLE content_reviews (
  id           BIGINT PRIMARY KEY AUTO_INCREMENT,
  entity_kind  ENUM('TEST_PAPER','TEST_SECTION','PASSAGE','QUESTION','COURSE','MODULE','LESSON') NOT NULL,
  entity_id    BIGINT NOT NULL,
  reviewer_id  BIGINT NOT NULL,
  status       ENUM('PENDING','APPROVED','REJECTED') NOT NULL DEFAULT 'PENDING',
  comment      TEXT,
  reviewed_at  DATETIME NULL,
  created_at   DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  INDEX idx_cr_entity (entity_kind, entity_id),
  CONSTRAINT fk_cr_reviewer FOREIGN KEY (reviewer_id) REFERENCES users(id)
) ENGINE=InnoDB;

CREATE TABLE audit_logs (
  id          BIGINT PRIMARY KEY AUTO_INCREMENT,
  user_id     BIGINT,
  action      VARCHAR(100) NOT NULL,                   -- CREATE_PAPER, PUBLISH_SECTION, GRADE_AI, UPDATE_SCORE...
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
('TEACHER','Giáo viên'),
('ADMIN','Quản trị viên');
