using System;
using System.Security.Cryptography;
using System.Text;
using LopFund.DAL;

namespace LopFund.BLL
{
    public class UserBLL   // <-- PHẢI public
    {
        private readonly UserDAL _dal = new UserDAL();

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

        private static string Sha256Hex(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}

