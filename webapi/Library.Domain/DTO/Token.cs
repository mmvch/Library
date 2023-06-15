using Library.Domain.Const;

namespace Library.Domain.DTO
{
	public class Token
	{
		public TokenType Type { get; set; }
		public string Data { get; set; }
	}
}
