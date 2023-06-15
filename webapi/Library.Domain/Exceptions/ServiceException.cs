using System.Net;

namespace Library.Domain.Exceptions
{
	public class ServiceException : Exception
	{
		public HttpStatusCode HttpStatusCode { get; set; }

		public ServiceException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
			: base(message)
		{
			HttpStatusCode = httpStatusCode;
		}
	}
}
