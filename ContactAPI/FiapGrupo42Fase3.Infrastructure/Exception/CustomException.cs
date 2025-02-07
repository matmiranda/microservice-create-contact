using System.Net;

namespace FiapGrupo42Fase3.Infrastructure.Exception
{
    public class CustomException : System.Exception
    {
        public HttpStatusCode StatusCode { get; }

        public CustomException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
