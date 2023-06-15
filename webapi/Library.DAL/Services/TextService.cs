using Library.DAL.Repositories;
using Library.Domain.Const;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Library.DAL.Services
{
	public class TextService : ITextService
	{
		private readonly IRepository<BookText, Guid> _repository;

		public TextService(IServiceProvider services)
		{
			_repository = services.GetRequiredService<IRepository<BookText, Guid>>();
		}

		public async Task<string> ReadAsync(IFormFile formFile)
		{
			ValidateText(formFile.FileName);

			try
			{
				using StreamReader reader = new(formFile.OpenReadStream());
				return await reader.ReadToEndAsync();
			}
			catch (Exception)
			{
				throw new ServiceException("Reading error: An unexpected error occurred while reading the file.");
			}
		}

		public async Task<IEnumerable<IEnumerable<string>>?> GetAsync(Guid id)
		{
			BookText bookText = await _repository.GetItemAsync(id) ??
				throw new ServiceException("The appropriate book does not exist.", HttpStatusCode.BadRequest);

			return bookText.Text == null ? null : SplitText(bookText.Text);
		}

		private static void ValidateText(string textName)
		{
			if (!FileExtension.TextExtensions.Contains(Path.GetExtension(textName)))
			{
				throw new ServiceException(
					$"Uploading error: Invalid file extension for text.\n"
					+ $"(Possible extensions: \"{string.Join("\" \"", FileExtension.TextExtensions)}\")",
					HttpStatusCode.UnsupportedMediaType);
			}
		}

		private static IEnumerable<IEnumerable<string>> SplitText(in string text)
		{
			IEnumerable<string> paragraphs = text.Split('\n')
				.Select(s => s.Trim())
				.Where(s => !string.IsNullOrEmpty(s));

			List<List<string>> pages = new();
			List<string> page = new();
			int pageSize = (int)PageSize.Medium;
			int currentSize = 0;

			foreach (var paragraph in paragraphs)
			{
				if (currentSize > pageSize)
				{
					pages.Add(page);
					page = new List<string>();
					currentSize = 0;
				}

				page.Add(paragraph);
				currentSize += paragraph.Length;
			}

			if (page.Any())
			{
				pages.Add(page);
			}

			return pages;
		}
	}
}
