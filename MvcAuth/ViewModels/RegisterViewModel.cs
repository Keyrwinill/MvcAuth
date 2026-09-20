using System.ComponentModel.DataAnnotations;

namespace MvcAuth.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		[StringLength(30)]
		public string Account { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		[StringLength(50)]
		public string Email { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password))]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required]
		[StringLength(30)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[StringLength(30)]
		public string LastName { get; set; } = string.Empty;

		[DataType(DataType.Date)]
		public DateTime? Birthday { get; set; }
	}
}