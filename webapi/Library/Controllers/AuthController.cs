using Library.DAL.Services;
using Library.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<Token> Register([FromBody] Credentials credentials)
		{
			return await _authService.RegisterAsync(credentials);
		}

		[HttpPost("login")]
		public async Task<Token> Login([FromBody] Credentials credentials)
		{
			return await _authService.LoginAsync(credentials);
		}
	}
}
