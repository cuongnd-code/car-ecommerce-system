namespace ManageCars.Models
{
	public class User
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string? Name { get; set; }
		public string? Email { get; set; }

		public string? Address { get; set; }

		public string? Phone { get; set; }

		public string? Username { get; set; }
		public string? PasswordHash { get; set; }

		public DateTime LastOnline { get; set; }

		public UserRole Role { get; set; } = UserRole.User;
		public enum UserRole
		{
			Admin,
			User
		}
	}
}
