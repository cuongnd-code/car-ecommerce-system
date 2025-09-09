using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ManageCars.Models.ViewModel
{
	public class RegisterViewModel 
	{


		public  string Id { get; set; }


		public string? AccountName { get; set; }
		public string? AccountPassword { get; set; }

		public string? ReAccountPassword { get; set; }

		public string? Name { get; set; }
		public string? Email { get; set; }

	}
}
