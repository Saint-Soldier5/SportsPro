using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Country
    {
		[Required]
		public string CountryID { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
