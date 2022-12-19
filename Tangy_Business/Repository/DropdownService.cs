using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tangy_Business.Repository.IRepository;
using Tangy_DataAccess.Data;

namespace Tangy_Business.Repository
{
	public class DropdownService : IDropdownService
	{
		private readonly ApplicationDbContext _dbContext;

		public DropdownService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public List<SelectListItem> ListofCities(int stateid)
		{
			try
			{
				var listofCities = (from cities in _dbContext.Cities
									where cities.StateId == stateid
									select new SelectListItem()
									{
										Text = cities.Name,
										Value = cities.CityId.ToString()
									}
					).ToList();

				listofCities.Insert(0, new SelectListItem()
				{
					Value = "",
					Text = "---Select---"
				});
				return listofCities;
			}
			catch (Exception)
			{
				throw;
			}
		}
	

		public List<SelectListItem> ListofCountries()
		{
			try
			{
				var listofCountries = (from countries in _dbContext.Countries
									   select new SelectListItem()
									   {
										   Text = countries.Name,
										   Value = countries.CountryId.ToString()
									   }
					).ToList();

				listofCountries.Insert(0, new SelectListItem()
				{
					Value = "",
					Text = "---Select---"
				});

				return listofCountries;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public List<SelectListItem> ListofStates(int countryId)
		{
			try
			{
				var listofstates = (from states in _dbContext.States
									where states.CountryId == countryId
									select new SelectListItem()
									{
										Text = states.Name,
										Value = states.StateId.ToString()
									}
					).ToList();
				listofstates.Insert(0, new SelectListItem()
				{
					Value = "",
					Text = "---Select---"
				});
				return listofstates;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
