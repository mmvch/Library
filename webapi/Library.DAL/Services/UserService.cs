using Library.DAL.Repositories;
using Library.Domain.Const;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace Library.DAL.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User, Guid> _userRepository;

		public UserService(IServiceProvider services)
		{
			_userRepository = services.GetRequiredService<IRepository<User, Guid>>();
		}

		public async Task<Guid> GetIdAsync(ClaimsPrincipal principal)
		{
			Guid id = ParseId(principal);

			return await _userRepository.GetItemAsync(id) != null ? id
				: throw new ServiceException("The appropriate user does not exist.", HttpStatusCode.BadRequest);
		}

		public async Task<User> GetAsync(Guid id)
		{
			return await _userRepository.GetItemAsync(id)
				?? throw new ServiceException("The appropriate user does not exist.", HttpStatusCode.BadRequest);
		}

		public async Task<User> GetAsync(ClaimsPrincipal principal)
		{
			Guid id = ParseId(principal);

			return await _userRepository.GetItemAsync(id)
				?? throw new ServiceException("The appropriate user does not exist.", HttpStatusCode.BadRequest);
		}

		public async Task<User> GetAsync(ClaimsPrincipal principal, Expression<Func<User, object?>> include)
		{
			Guid id = ParseId(principal);

			return await _userRepository.GetItemAsync(user => user.Id == id, include)
				?? throw new ServiceException("The appropriate user does not exist.", HttpStatusCode.BadRequest); ;
		}

		public async Task ValidateUserAsync(ClaimsPrincipal principal, Guid userId)
		{
			User user = await GetAsync(principal, user => user.Roles);

			if (user.Roles.Select(role => role.Name).Contains(LibraryRoles.Admin))
			{
				return;
			}

			if (userId != user.Id)
			{
				throw new ServiceException("Invalid user credentials. Only the book author can edit it.",
					HttpStatusCode.BadRequest);
			}
		}

		private static Guid ParseId(ClaimsPrincipal principal)
		{
			return Guid.Parse(principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value
				?? throw new ServiceException("Invalid user token.", HttpStatusCode.BadRequest));
		}
	}
}
