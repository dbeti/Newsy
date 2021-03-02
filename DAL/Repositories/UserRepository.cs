using DAL;
using DAL.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	/// Implementation of the basic user operations on the database using entity framework core. 
	public class UserRepository : IUserRepository
	{
		private readonly NewsyDbContext _newsyDbContext;
		public UserRepository(NewsyDbContext newsyDbContext)
		{
			_newsyDbContext = newsyDbContext;
		}

		/// <inheritdoc/>
		public async Task<User> GetUserByIdAsync(string id)
		{
			return await _newsyDbContext.Users.FindAsync(id);
		}
	}
}
