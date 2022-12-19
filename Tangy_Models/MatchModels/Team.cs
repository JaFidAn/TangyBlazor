using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Models.MatchModels
{
	public class Team
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CountryId { get; set; }
		public int LeagueId { get; set; }
	}
}
