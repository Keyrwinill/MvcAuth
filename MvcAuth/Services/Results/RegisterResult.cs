using MvcAuth.Models;

namespace MvcAuth.Services.Results
{
	public class RegisterResult
	{
		public bool Success { get; set; }

		public string? ErrorCode { get; set; }

		public User? User { get; set; }
	}
}