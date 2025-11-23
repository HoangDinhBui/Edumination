-- =========================================================
-- SCRIPT CẬP NHẬT PASSWORD SANG BCRYPT
-- =========================================================
-- Script này sẽ update passwords từ SHA256 sang BCrypt format
-- Chạy script này TRƯỚC KHI sử dụng login với BCrypt

USE IELTSLearning;
GO

-- =========================================================
-- 1. Backup bảng Users trước khi update
-- =========================================================
IF OBJECT_ID('Users_Backup', 'U') IS NOT NULL
    DROP TABLE Users_Backup;
GO

SELECT * INTO Users_Backup FROM Users;
GO

PRINT 'Đã backup bảng Users vào Users_Backup';
GO

-- =========================================================
-- 2. Update passwords sang BCrypt format
-- =========================================================
-- Lưu ý: BCrypt hash có format: $2a$12$... (60 ký tự)
-- Password gốc: 123456
-- BCrypt hash của "123456" với work factor 12:

-- Update Student account
UPDATE Users 
SET PasswordHash = '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYzpLhSiK3u'
WHERE Email = 'student@test.com';

-- Update Teacher account  
UPDATE Users
SET PasswordHash = '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYzpLhSiK3u'
WHERE Email = 'teacher@ielts.com';

-- Update Admin account (nếu password là 123456)
-- Nếu admin password khác, cần generate BCrypt hash riêng
UPDATE Users
SET PasswordHash = '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYzpLhSiK3u'
WHERE Email = 'buidinhhoang1910@gmail.com';

GO

PRINT 'Đã update passwords sang BCrypt format';
GO

-- =========================================================
-- 3. Verify update
-- =========================================================
SELECT 
    Email,
    LEFT(PasswordHash, 20) + '...' AS PasswordHash_Preview,
    LEN(PasswordHash) AS Hash_Length,
    Role,
    IsActive
FROM Users
ORDER BY CreatedAt;

GO

PRINT '===========================================';
PRINT 'CẬP NHẬT HOÀN TẤT!';
PRINT '===========================================';
PRINT '';
PRINT 'Thông tin đăng nhập:';
PRINT '1. Student: student@test.com / 123456';
PRINT '2. Teacher: teacher@ielts.com / 123456';
PRINT '3. Admin: buidinhhoang1910@gmail.com / 123456';
PRINT '';
PRINT 'BCrypt Hash Format: $2a$12$...';
PRINT 'Hash Length: 60 characters';
PRINT '';
PRINT 'Nếu cần rollback, chạy:';
PRINT 'DROP TABLE Users;';
PRINT 'SELECT * INTO Users FROM Users_Backup;';
GO
