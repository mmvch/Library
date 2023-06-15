using Library.Domain.Const;
using Library.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Net;
using System.Runtime.Versioning;

namespace Library.DAL.Services
{
	public class ImageService : IImageService
	{
		private readonly IConfiguration _configuration;

		public ImageService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[SupportedOSPlatform("windows")]
		public async Task<string> ReplaceAsync(IFormFile formFile, string imageName)
		{
			ValidateImage(formFile.FileName);
			await DeleteAsync(imageName);
			return await SaveImageAsync(formFile);
		}

		[SupportedOSPlatform("windows")]
		public async Task<string> UploadAsync(IFormFile formFile)
		{
			ValidateImage(formFile.FileName);
			return await SaveImageAsync(formFile);
		}

		public async Task DeleteAsync(string fileName)
		{
			try
			{
				string filePath = Path.Combine(_configuration.GetSection("WebRootPath").Value!, $"uploads\\images", fileName);

				if (File.Exists(filePath))
				{
					await Task.Run(() => File.Delete(filePath));
				}
			}
			catch (Exception)
			{
				throw new ServiceException("Deleting error: An unexpected error occurred while deleting the image.");
			}
		}

		private static void ValidateImage(string imageName)
		{
			if (!FileExtension.ImageExtensions.Contains(Path.GetExtension(imageName)))
			{
				throw new ServiceException(
					$"Uploading error: Invalid file extension for image.\n"
					+ $"(Possible extensions: \"{string.Join("\" \"", FileExtension.ImageExtensions)}\")",
					HttpStatusCode.UnsupportedMediaType);
			}
		}

		[SupportedOSPlatform("windows")]
		private async Task<string> SaveImageAsync(IFormFile formFile)
		{
			try
			{
				string fileName = Guid.NewGuid() + FileExtension.Default(FileType.Image);
				string filePath = Path.Combine(_configuration.GetSection("WebRootPath").Value!, $"uploads\\images", fileName);

				using (Stream stream = formFile.OpenReadStream())
				using (Image image = Image.FromStream(stream).GetThumbnailImage(500, 500, null, IntPtr.Zero))
				{
					await Task.Run(() => image.Save(filePath));
				}

				return fileName;
			}
			catch (Exception)
			{
				throw new ServiceException("Uploading error: An unexpected error occurred while uploading the image.");
			}
		}
	}
}
