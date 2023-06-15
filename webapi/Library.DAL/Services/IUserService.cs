using Library.Domain.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Library.DAL.Services
{
	public interface IUserService
	{
		Task<Guid> GetIdAsync(ClaimsPrincipal principal);

		Task<User> GetAsync(Guid id);

		Task<User> GetAsync(ClaimsPrincipal principal);

		Task<User> GetAsync(ClaimsPrincipal principal, Expression<Func<User, object?>> include);

		Task ValidateUserAsync(ClaimsPrincipal principal, Guid userId);
	}
}
