namespace ManageCars.Models.ViewModel
{
	public class RegisterViewModel
	{


		public string Id { get; set; }


		public string? Username { get; set; }
		public string? PasswordHash { get; set; }

		public string? ReAccountPassword { get; set; }

		public string? Name { get; set; }
		public string? Email { get; set; }

	}
}
