namespace MvcAuth.ViewModels
{
	public class UserListItemViewModel
	{
		public Guid Oid { get; set; }

		public string Account { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string FirstName { get; set; } = string.Empty;

		public string LastName { get; set; } = string.Empty;

		public DateTime? Birthday { get; set; }

		public string Role { get; set; } = string.Empty;

		public DateTime CreatedDate { get; set; }
	}
}