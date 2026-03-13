using System;
using System.Security.Cryptography;
using System.Text;
using LopFund.DAL;

namespace LopFund.BLL
{
    public class UserBLL
    {
        private readonly UserDAL _dal = new UserDAL();

        // ===== LOGIN =====
        public User Login(string email, string password)
        {
            email = (email ?? "").Trim();
            password = password ?? "";

            var u = _dal.GetByEmail(email);
            if (u == null) return null;

            var inputHash = Sha256Hex(password);
            return string.Equals(u.PasswordHash, inputHash, StringComparison.OrdinalIgnoreCase)
                ? u
                : null;
        }

        // ===== REGISTER =====
        public User Register(string fullName, string email, string phone, string password, string role = "User")
        {
            fullName = (fullName ?? "").Trim();
            email = (email ?? "").Trim();
            phone = (phone ?? "").Trim();
            password = password ?? "";

            if (string.IsNullOrWhiteSpace(fullName))
                throw new Exception("Họ tên không được trống.");

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email không được trống.");

            if (password.Length < 6)
                throw new Exception("Mật khẩu tối thiểu 6 ký tự.");

            if (_dal.ExistsEmail(email))
                throw new Exception("Email đã tồn tại.");

            var user = new User
            {
                FullName = fullName,
                Email = email,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                PasswordHash = Sha256Hex(password),
                Role = string.IsNullOrWhiteSpace(role) ? "User" : role
            };

            return _dal.Insert(user);
        }

        // ===== HASH =====
        private static string Sha256Hex(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? ""));
                var sb = new StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}