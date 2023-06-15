using Library.Domain.DTO;
using Library.Domain.Models;
using System.Linq.Expressions;

namespace Library.DAL.Services
{
	public interface IBookService
	{
		Task<PartialData<Book>> GetBookListAsync(BookFilter bookFilter, params Expression<Func<Book, object?>>[] includes);
		Task<Book> GetAsync(Guid id);
		Task<Book> GetAsync(Guid id, params Expression<Func<Book, object?>>[] includes);
		Task<Book> CreateAsync(Book book);
		Task<Book> UpdateAsync(Book book);
		Task DeleteAsync(Guid id);
	}
}
