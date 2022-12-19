using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models.LearnBlazorModels
{
	public class State
	{
		
		public int StateId { get; set; }
		[MaxLength(100)]
		public string Name { get; set; }
		public int CountryId { get; set; }
	}
}
