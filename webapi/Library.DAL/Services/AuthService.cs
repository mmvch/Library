using Library.DAL.Repositories;
using Library.Domain.Const;
using Library.Domain.DTO;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Library.DAL.Services
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;
		private readonly IRepository<User, Guid> _userRepository;
		private readonly IRepository<Role, Guid> _roleRepository;

		public AuthService(IConfiguration configuration, IRepository<User, Guid> userRepository, IRepository<Role, Guid> roleRepository)
		{
			_configuration = configuration;
			_userRepository = userRepository;
			_roleRepository = roleRepository;
		}

		public async Task<Token> RegisterAsync(Credentials credentials)
		{
			try
			{
				ValidateCredentials(credentials);
				GeneratePasswordHash(credentials.Password, out byte[] passwordHash, out byte[] passwordSalt);
				Role role = await _roleRepository.GetItemAsync(role => role.Name == LibraryRoles.Author);

				User user = new()
				{
					Login = credentials.Login,
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt,
					Roles = new List<Role> { role }
				};

				await _userRepository.CreateAsync(user);
				await _userRepository.SaveAsync();

				return await LoginAsync(credentials);
			}
			catch (ServiceException)
			{
				throw;
			}
			catch
			{
				throw new ServiceException("Login is already in use.", HttpStatusCode.BadRequest);
			}
		}

		public async Task<Token> LoginAsync(Credentials credentials)
		{
			try
			{
				User user = await _userRepository.GetItemAsync(user => user.Login == credentials.Login, user => user.Roles);

				if (VerifyPasswordHash(credentials.Password, user.PasswordHash, user.PasswordSalt))
				{
					return CreateToken(user);
				}

				throw new Exception();
			}
			catch
			{
				throw new ServiceException("Invalid login or password.", HttpStatusCode.BadRequest);
			}
		}

		private static void ValidateCredentials(Credentials credentials)
		{
			if (credentials.Login.Length < 5)
			{
				throw new ServiceException("Login must have at least 5 characters.", HttpStatusCode.BadRequest);
			}

			if (credentials.Password.Length < 5)
			{
				throw new ServiceException("Password must have at least 5 characters.", HttpStatusCode.BadRequest);
			}
		}

		private static void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512();
			passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			passwordSalt = hmac.Key;
		}

		private static bool VerifyPasswordHash(string password, in byte[] passwordHash, in byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512(passwordSalt);
			byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(passwordHash);
		}

		private Token CreateToken(User user)
		{
			List<Claim> claims = new()
			{
				new Claim(ClaimTypes.Name, user.Login.ToString()
					?? throw new ServiceException("Invalid user name.", HttpStatusCode.BadRequest)),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()
					?? throw new ServiceException("Invalid user identifier.", HttpStatusCode.BadRequest))
			};

			foreach (string role in user.Roles.Select(role => role.Name))
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value!));

			SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha512Signature);

			JwtSecurityToken jwtToken = new(
				_configuration.GetSection("Jwt:Issuer").Value,
				_configuration.GetSection("Jwt:Audience").Value,
				claims: claims,
				signingCredentials: credentials);

			return new Token()
			{
				Type = TokenType.Jwt,
				Data = new JwtSecurityTokenHandler().WriteToken(jwtToken)
			};
		}
	}
}
