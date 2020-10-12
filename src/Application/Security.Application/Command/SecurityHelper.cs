using System;

namespace Security.Application.Command
{
  public static class SecurityHelper
  {
    public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
    {
      if (password == null) throw new ArgumentNullException(nameof(password));
      if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

      byte[] hash = Convert.FromBase64String(storedHash);
      byte[] salt = Convert.FromBase64String(storedSalt);

      if (hash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
      if (salt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

      using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
      {
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (int i = 0; i < computedHash.Length; i++)
        {
          if (computedHash[i] != hash[i]) return false;
        }
      }

      return true;
    }

    public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
    {
      if (password == null) throw new ArgumentNullException(nameof(password));
      if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = Convert.ToBase64String(hmac.Key);
        passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
      }
    }
  }
}
