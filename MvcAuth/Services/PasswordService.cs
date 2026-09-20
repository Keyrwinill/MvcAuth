using System.Security.Cryptography;

namespace MvcAuth.Services
{
	public class PasswordService
	{
		public bool VerifyPassword(string password, string storedPasswordHash)
		{
			// Stored format:
			// iterations.salt.hash
			// Example:
			// 100000.<Base64 salt>.<Base64 hash>
			var parts = storedPasswordHash.Split('.');

			if (parts.Length != 3)
			{
				return false;
			}

			// Read the iteration count that was used when
			// the password was originally hashed.
			if (!int.TryParse(parts[0], out int iterations))
			{
				return false;
			}

			byte[] salt;
			byte[] expectedHash;

			try
			{
				// Retrieve the ORIGINAL salt and hash from the database.
				// We do NOT generate a new salt when verifying a password.
				salt = Convert.FromBase64String(parts[1]);
				expectedHash = Convert.FromBase64String(parts[2]);
			}
			catch (FormatException)
			{
				return false;
			}

			// Hash the password entered during login using the
			// same salt and settings used for the original password.
			var actualHash = Rfc2898DeriveBytes.Pbkdf2(
				password,
				salt,
				iterations,
				HashAlgorithmName.SHA256,
				expectedHash.Length);

			// Compare:
			// newly calculated hash vs. hash stored in the database.
			return CryptographicOperations.FixedTimeEquals(
				actualHash,
				expectedHash);
		}

		public string HashPassword(string password)
		{
			const int iterations = 100000;
			const int saltSize = 16;
			const int hashSize = 32;

			// Generate a NEW random salt when creating a password.
			byte[] salt = RandomNumberGenerator.GetBytes(saltSize);

			// Generate the password hash using:
			// password + salt + iterations + SHA256
			byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
				password,
				salt,
				iterations,
				HashAlgorithmName.SHA256,
				hashSize);

			// Store everything needed for future verification.
			return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
		}
	}
}