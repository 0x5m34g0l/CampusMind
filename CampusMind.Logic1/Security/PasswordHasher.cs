using BCrypt.Net;

namespace CampusMind.Logic1.Security
{
    public static class PasswordHasher
    {
        public static string Hash(string passwordPlain)
        {
            return BCrypt.Net.BCrypt.HashPassword(passwordPlain);
        }

        public static bool Verify(string passwordPlain, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(passwordPlain, storedHash);
        }
    }
}
