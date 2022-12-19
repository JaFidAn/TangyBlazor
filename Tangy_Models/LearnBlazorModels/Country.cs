using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models.LearnBlazorModels
{
	public class Country
	{
		[Key]
		public int CountryId { get; set; }
		[MaxLength(150)]
		public string Name { get; set; }
	}
}
