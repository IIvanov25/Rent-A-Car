using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Rent_A_Car.Models
{
	public class User : IdentityUser
	{
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EGN { get; set; }
		public string PhoneNumber { get; set; }
		public ICollection<Request> Requests { get; set; }
		public User()
		{

		}
	}
}
