using DAL.Models.EntityModels;
using System.Threading.Tasks;

namespace DAL.Repositories
{
	public interface IUserRepository
	{
		/// <summary>
		/// Gets user by the id.
		/// </summary>
		/// <param name="id">Id of the user that will be retrieved.</param>
		/// <returns>Populated user.</returns>
		Task<User> GetUserByIdAsync(string id);
	}
}
