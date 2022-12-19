using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangy_Models.LearnBlazorModels;

namespace Tangy_DataAccess.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<Country> Countries { get; set; }
		public DbSet<State> States { get; set; }
		public DbSet<City> Cities { get; set; }
	}
}
