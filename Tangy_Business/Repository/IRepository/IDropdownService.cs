using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tangy_Business.Repository.IRepository
{
	public interface IDropdownService
	{
		List<SelectListItem> ListofCountries();
		List<SelectListItem> ListofStates(int countryId);
		List<SelectListItem> ListofCities(int stateid);
	}
}
