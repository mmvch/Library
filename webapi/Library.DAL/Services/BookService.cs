using Library.DAL.Repositories;
using Library.Domain.DTO;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Net;

namespace Library.DAL.Services
{
	public class BookService : IBookService
	{
		private readonly IRepository<Book, Guid> _bookRepository;

		public BookService(IServiceProvider services)
		{
			_bookRepository = services.GetRequiredService<IRepository<Book, Guid>>();
		}

		public async Task<PartialData<Book>> GetBookListAsync(BookFilter bookFilter, params Expression<Func<Book, object?>>[] includes)
		{
			Expression<Func<Book, bool>>? predicate = bookFilter.Name.IsNullOrEmpty() ? null :
				(book => book.Name.Contains(bookFilter.Name!));

			return await _bookRepository.GetItemListAsync(predicate,
				(bookFilter.CurrentPage - 1) * bookFilter.PageSize, bookFilter.PageSize, includes);
		}

		public async Task<Book> GetAsync(Guid id)
		{
			return await _bookRepository.GetItemAsync(id) ??
				throw new ServiceException("The appropriate book does not exist.", HttpStatusCode.BadRequest);
		}

		public async Task<Book> GetAsync(Guid id, params Expression<Func<Book, object?>>[] includes)
		{
			return await _bookRepository.GetItemAsync(item => item.Id == id, includes) ??
				throw new ServiceException("The appropriate book does not exist.", HttpStatusCode.BadRequest);
		}

		public async Task<Book> CreateAsync(Book book)
		{
			try
			{
				await _bookRepository.CreateAsync(book);
				await _bookRepository.SaveAsync();

				return book;
			}
			catch (Exception)
			{
				throw new ServiceException("An unexpected error occurred while creating the book.", HttpStatusCode.BadRequest);
			}
		}

		public async Task<Book> UpdateAsync(Book book)
		{
			try
			{
				_bookRepository.Update(book);
				await _bookRepository.SaveAsync();

				return book;
			}
			catch (Exception)
			{
				throw new ServiceException("An unexpected error occurred while updating the book.", HttpStatusCode.BadRequest);
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			try
			{
				await _bookRepository.DeleteAsync(id);
				await _bookRepository.SaveAsync();
			}
			catch (Exception)
			{
				throw new ServiceException("An unexpected error occurred while deleting the book.", HttpStatusCode.BadRequest);
			}
		}
	}
}
