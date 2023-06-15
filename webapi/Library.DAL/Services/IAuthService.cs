using Library.Domain.DTO;

namespace Library.DAL.Services
{
	public interface IAuthService
	{
		Task<Token> RegisterAsync(Credentials credentials);
		Task<Token> LoginAsync(Credentials credentials);
	}
}
