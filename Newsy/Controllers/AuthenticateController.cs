using BLL.Models.Dtos;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newsy.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Newsy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(IAuthService authService,
            ILogger<AuthenticateController> logger,
            IConfiguration configuration)
        {
            _authService = authService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterBindingModel model)
        {
            _logger.LogInformation($"User {model.Username} registration started.");
            var isRegistered = await _authService.IsRegisteredAsync(model.Username); 
            if (isRegistered)
            {
                _logger.LogWarning($"User {model.Username} already exists! Registration failed.");
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            var user = new UserDto
            {
                Email = model.Email,
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsAuthor = model.IsAuthor,
                Password = model.Password
            };

            var isCreated = await _authService.RegisterAsync(user);
            if (!isCreated)
			{
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            _logger.LogInformation($"User {user.UserName} registered successfully!");

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginBindingModel model)
        {
            var jwtDto = await _authService.LoginAsync(model.UserName, model.Password);
            if (jwtDto == null)
			{
                return Unauthorized();
            }

            return Ok(jwtDto);
        }
    }
}
