using System;
using System.ComponentModel.DataAnnotations;

namespace SportsPro.Models
{
    public class Technician
    {
		public int TechnicianID { get; set; }	   

		[Required(ErrorMessage = "Please enter a Name")]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.EmailAddress,ErrorMessage = "Please enter a Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Please enter a phone number")]
		[RegularExpression("^[(]{0,1}[0-9]{3}[)]{0,1}[0-9]{3}[-]{0,1}[0-9]{4}$", ErrorMessage = "Phone number must be in (999)999-9999 format")]
		public string Phone { get; set; }
	}
}
