using System.Net;

namespace Application.Exceptions
{
    public class HttpException: Exception
    {
        public HttpException(int statusCode, string message): base(message) => StatusCode = (HttpStatusCode)statusCode;
        public HttpException(HttpStatusCode statusCode, string message): base(message) => StatusCode = statusCode;
        
        public HttpStatusCode StatusCode { get; }
    }
}