namespace ManageCars.Models
{
    public class Account
    {
        public required string Id { get; set; }

        public required string? AccountName { get; set; }
        public required string AccountPassword { get; set; }
		public string?  role { get; set; } // e.g., "admin", "user", etc.
		public string? email { get; set; }


        public User? user { get; set; } 


    }
}
