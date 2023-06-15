namespace Library.Domain.Const
{
	public static class FileExtension
	{
		#region image
		public static readonly string BMP = ".bmp";
		public static readonly string JPG = ".jpg";
		public static readonly string JPE = ".jpe";
		public static readonly string PNG = ".png";

		public static IEnumerable<string> ImageExtensions
		{
			get
			{
				yield return BMP;
				yield return JPG;
				yield return JPE;
				yield return PNG;
			}
		}
		#endregion

		#region text
		public static readonly string TXT = ".txt";

		public static IEnumerable<string> TextExtensions
		{
			get
			{
				yield return TXT;
			}
		}
		#endregion

		public static string Default(FileType fileType)
		{
			return fileType switch
			{
				FileType.Image => BMP,
				FileType.Text => TXT,
				_ => string.Empty
			};
		}
	}
}
