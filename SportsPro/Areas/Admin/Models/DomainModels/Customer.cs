using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SportsPro.Models
{
    public class Customer
    {
		[Required(ErrorMessage ="Please select a customer.")]
		public int CustomerID { get; set; }

        [Required(ErrorMessage = "Please enter a first name.")]
		[StringLength(51)]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Please enter a last name.")]
		[StringLength(51)]
		public string LastName { get; set; }

		[Required]
		[StringLength(51)]
		public string Address { get; set; }

		[Required]
		[StringLength(51)]
		public string City { get; set; }

		[Required]
		[StringLength(51)]
		public string State { get; set; }

		[Required]
		[StringLength(21,ErrorMessage ="Postal code should less than 21 characters.")]
        [RegularExpression("^[a-zA-Z0-9]+$",
				ErrorMessage = "Postal code may not contain special characters.")]
        public string PostalCode { get; set; }

		[Required]
		public string CountryID { get; set; }

		
		public Country Country { get; set; }

		[RegularExpression("^[(]{0,1}[0-9]{3}[)]{0,1}[0-9]{3}[-]{0,1}[0-9]{4}$",ErrorMessage ="Phone number must be in (999)999-9999 format")]
		public string Phone { get; set; }

		[Required]
		[StringLength(51)]
		[DataType(DataType.EmailAddress,ErrorMessage ="Please enter a valid email address.")]
	    //[UniqueEmail]
		//[Remote(name)]
		public string Email { get; set; }

		public string FullName => FirstName + " " + LastName;   // read-only property

        public ICollection<CustProd> CustProds { get; set; }
    }
}