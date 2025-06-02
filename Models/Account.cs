namespace firstWeb.Models
{
    public class Account
    {
        public required string Id { get; set; }

        public required string AccountName { get; set; }
        public required string AccountPassword { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

    }
}
