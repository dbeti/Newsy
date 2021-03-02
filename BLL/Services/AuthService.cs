using BLL.Models.Dtos;
using DAL.Models.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class AuthService : IAuthService
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;
		private readonly ILogger<AuthService> _logger;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<User> userManager, 
			RoleManager<IdentityRole> roleManager,
			ILogger<AuthService> logger,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_logger = logger;
			_configuration = configuration;
		}

		public async Task<bool> RegisterAsync(UserDto userDto)
		{
			var user = new User
			{
				Email = userDto.Email,
				UserName = userDto.UserName,
				FirstName = userDto.FirstName,
				LastName = userDto.LastName,
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var identityResult = await _userManager.CreateAsync(user, userDto.Password);
			if (!identityResult.Succeeded)
			{
				var errorCodes = string.Join(",", identityResult.Errors.Select(x => x.Code));
				_logger.LogError($"User {userDto.UserName} creation failed with errors {errorCodes}");
				return false;
			}

			if (!await _roleManager.RoleExistsAsync(UserRoles.Reader))
			{
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.Reader));
			}

			if (!await _roleManager.RoleExistsAsync(UserRoles.Author))
			{
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.Author));
			}

			if (userDto.IsAuthor)
			{
				await _userManager.AddToRoleAsync(user, UserRoles.Author);
			}
			else
			{
				await _userManager.AddToRoleAsync(user, UserRoles.Reader);
			}


			return true;
		}

		public async Task<bool> IsRegisteredAsync(string username)
		{
			var registeredUser = await _userManager.FindByNameAsync(username);
			return registeredUser != null;
		}

		public async Task<JwtDto> LoginAsync(string userName, string password)
		{
			var user = await _userManager.FindByNameAsync(userName);

			if (user == null || !await _userManager.CheckPasswordAsync(user, password))
			{
				return null; 
			}

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			return new JwtDto
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = token.ValidTo
			};
		}
	}
}
