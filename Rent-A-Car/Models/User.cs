using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Rent_A_Car.Models
{
	public class User : IdentityUser
	{
		[StringLength(30, MinimumLength = 12)]
		public string Password { get; set; }
		[StringLength(30, MinimumLength = 3)]
		public string FirstName { get; set; }
		[StringLength(30, MinimumLength = 3)]
		public string LastName { get; set; }
		[StringLength(10, MinimumLength = 10)]
		public string EGN { get; set; }
		[StringLength(10, MinimumLength = 10)]
		public string PhoneNumber { get; set; }
		public ICollection<Request> Requests { get; set; }
		public string DisplayInfo => $"{FirstName} {LastName} - {EGN}";
		public User()
		{

		}
	}
}
