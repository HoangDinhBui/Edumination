using IELTS.DTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IELTS.BLL
{
    /// <summary>
    /// Helper class để tạo và validate JWT tokens
    /// </summary>
    public static class JwtHelper
    {
        // Secret key - trong production nên lưu trong config/environment variable
        private const string SECRET_KEY = "EduminationIELTS2024SecretKeyForJWTTokenGeneration";
        private const string ISSUER = "EduminationIELTS";
        private const string AUDIENCE = "EduminationUsers";
        private const int TOKEN_EXPIRY_HOURS = 24; // Token hết hạn sau 24 giờ

        /// <summary>
        /// Tạo JWT token cho user
        /// </summary>
        public static string GenerateToken(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Tạo claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("IsActive", user.IsActive.ToString())
            };

            // Thêm phone nếu có
            if (!string.IsNullOrEmpty(user.Phone))
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone));

            // Tạo token
            var token = new JwtSecurityToken(
                issuer: ISSUER,
                audience: AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(TOKEN_EXPIRY_HOURS),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Validate JWT token và trả về claims
        /// </summary>
        public static ClaimsPrincipal ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = ISSUER,
                    ValidAudience = AUDIENCE,
                    IssuerSigningKey = securityKey,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Token validation failed", ex);
            }
        }

        /// <summary>
        /// Lấy UserDTO từ token
        /// </summary>
        public static UserDTO GetUserFromToken(string token)
        {
            var principal = ValidateToken(token);
            
            return new UserDTO
            {
                Id = long.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                Email = principal.FindFirst(ClaimTypes.Email)?.Value,
                FullName = principal.FindFirst(ClaimTypes.Name)?.Value,
                Role = principal.FindFirst(ClaimTypes.Role)?.Value,
                Phone = principal.FindFirst(ClaimTypes.MobilePhone)?.Value,
                IsActive = bool.Parse(principal.FindFirst("IsActive")?.Value ?? "false")
            };
        }

        /// <summary>
        /// Kiểm tra token có hợp lệ không
        /// </summary>
        public static bool IsTokenValid(string token)
        {
            try
            {
                ValidateToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
