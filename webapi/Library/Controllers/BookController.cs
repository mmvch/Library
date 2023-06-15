using Library.DAL.Services;
using Library.Domain.DTO;
using Library.Domain.Models;
using Library.WebModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookController : ControllerBase
	{
		private readonly IBookService _bookService;
		private readonly IImageService _imageService;
		private readonly ITextService _textService;
		private readonly IUserService _userService;

		public BookController(IBookService bookService, IImageService imageService,
			ITextService textService, IUserService userService)
		{
			_bookService = bookService;
			_imageService = imageService;
			_textService = textService;
			_userService = userService;
		}

		[HttpGet]
		public async Task<PartialData<Book>> List([FromQuery] BookFilter bookFilter)
		{
			return await _bookService.GetBookListAsync(bookFilter, book => book.User);
		}

		[HttpGet("{id}")]
		public async Task<Book> Get(Guid id)
		{
			return await _bookService.GetAsync(id, book => book.User);
		}

		[HttpPost, Authorize]
		public async Task<Book> Create([FromForm] WebBook webBook)
		{
			Book book = new()
			{
				Id = Guid.NewGuid(),
				Name = webBook.Name,
				Description = webBook.Description,
				CreationDate = DateTime.Now,
				UserId = await _userService.GetIdAsync(User)
			};

			if (webBook.Image != null)
			{
				book.Image = await _imageService.UploadAsync(webBook.Image);
			}

			if (webBook.BookText != null)
			{
				book.BookText = new BookText()
				{
					Text = await _textService.ReadAsync(webBook.BookText)
				};
			}

			return await _bookService.CreateAsync(book);
		}

		[HttpPut, Authorize]
		public async Task<Book> Update([FromForm] WebBook webBook)
		{
			Book book = await _bookService.GetAsync(webBook.Id);
			await _userService.ValidateUserAsync(User, book.UserId);

			book.Name = webBook.Name;
			book.Description = webBook.Description;

			if (webBook.Image != null)
			{
				book.Image = book.Image != null
					? await _imageService.ReplaceAsync(webBook.Image, book.Image)
					: await _imageService.UploadAsync(webBook.Image);
			}

			if (webBook.BookText != null)
			{
				book.BookText = new BookText()
				{
					Text = await _textService.ReadAsync(webBook.BookText)
				};
			}

			return await _bookService.UpdateAsync(book);
		}

		[HttpDelete("{id}"), Authorize]
		public async Task Delete(Guid id)
		{
			Book book = await _bookService.GetAsync(id);
			await _userService.ValidateUserAsync(User, book.UserId);

			if (book.Image != null)
			{
				await _imageService.DeleteAsync(book.Image);
			}

			await _bookService.DeleteAsync(id);
		}
	}
}
