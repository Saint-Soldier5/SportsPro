using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Product
    {
		public int ProductID { get; set; }

		[Required(ErrorMessage = "Please enter a Product Code")]
		public string ProductCode { get; set; }

		[Required(ErrorMessage ="Please enter a Name")]
		public string Name { get; set; }
		
		[Range(0.01, 1000000,ErrorMessage = "Please enter a value greater than or equal to 0.01")]
		[Column(TypeName = "decimal(8,2)")]
		public decimal YearlyPrice { get; set; }

		public DateTime ReleaseDate { get; set; } = DateTime.Now;
		public ICollection<CustProd> CustProds { get; set; }
	}
}
