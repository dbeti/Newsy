using BLL.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public interface IAuthService
	{
		/// <summary>
		/// Checks if user is already registered.
		/// </summary>
		/// <param name="username">Username of the user that will be checked.</param>
		/// <returns></returns>
		Task<bool> IsRegisteredAsync(string username);
		/// <summary>
		/// Performs registration of the user in the system.
		/// </summary>
		/// <param name="user">User that will be registered.</param>
		/// <returns>True if user is registered successfuly.</returns>
		Task<bool> RegisterAsync(UserDto user);
		/// <summary>
		/// Performs login of the user in the system.
		/// </summary>
		/// <param name="userName">User name of the user that wants to login.</param>
		/// <param name="password">Password of the user that wants to login.</param>
		/// <returns>Token and the expiration date of the logged in user.</returns>
		Task<JwtDto> LoginAsync(string userName, string password);
	}
}
