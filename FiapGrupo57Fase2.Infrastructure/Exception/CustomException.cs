using System.Net;

namespace FiapGrupo57Fase2.Infrastructure.Exception
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
