using Microsoft.CodeAnalysis.Elfie.Model;
using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.Models
{
	public class Car
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(30, MinimumLength = 3)]
		public string Brand { get; set; }
		[Required]
		[StringLength(30, MinimumLength = 2 )]
		public string Model { get; set; }
		[Required]
		[Range(1990, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
		public int YearOfProduction { get; set; }
		[Required]
		[Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
		public int Seats { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		[Range(50, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
		public double PricePerDay { get; set; }
		public string DisplayInfo => $"{Brand} {Model}";
	}
}
