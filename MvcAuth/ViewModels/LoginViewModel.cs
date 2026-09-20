using System.ComponentModel.DataAnnotations;

namespace MvcAuth.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		public string Account { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		public bool RememberMe { get; set; }
	}
}