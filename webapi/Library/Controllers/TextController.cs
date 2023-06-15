using Library.DAL.Services;
using Library.WebModels;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TextController : ControllerBase
	{
		private readonly ITextService _textService;

		public TextController(ITextService textService)
		{
			_textService = textService;
		}

		[HttpGet("{id}")]
		public async Task<WebText> Get(Guid id)
		{
			return new WebText()
			{
				Id = id,
				Pages = await _textService.GetAsync(id)
			};
		}
	}
}
