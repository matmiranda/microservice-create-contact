using FiapGrupo57Fase2.DTO.Response;
using FiapGrupo57Fase2.Infrastructure.Exception;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Text.Json;

namespace FiapGrupo57Fase2.Test.Infrastructure
{
    [TestFixture]
    public class ExceptionMiddlewareTests
    {
        private Mock<RequestDelegate> _nextMock;
        private DefaultHttpContext _httpContext;
        private ExceptionMiddleware _middleware;

        [SetUp]
        public void SetUp()
        {
            _nextMock = new Mock<RequestDelegate>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Response.Body = new MemoryStream();
            _middleware = new ExceptionMiddleware(_nextMock.Object);
        }

        [Test]
        public async Task InvokeAsync_ShouldHandleCustomException()
        {
            // Arrange
            var customException = new CustomException(HttpStatusCode.BadRequest, "Custom error");
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(customException);

            // Act
            await _middleware.InvokeAsync(_httpContext);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(_httpContext.Response.ContentType, Is.EqualTo("application/json"));
                Assert.That(_httpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            });

            _httpContext.Response.Body.Seek(0, System.IO.SeekOrigin.Begin);
            var responseBody = new System.IO.StreamReader(_httpContext.Response.Body).ReadToEnd();

            Assert.That(responseBody, Is.EqualTo("{\"message\":\"Custom error\"}"));
        }


        [Test]
        public async Task InvokeAsync_GenericException_ReturnsInternalServerErrorResponse()
        {
            // Arrange
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(new Exception("Generic error message"));

            // Act
            await _middleware.InvokeAsync(_httpContext);

            // Assert
            Assert.That(_httpContext.Response.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            _httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(_httpContext.Response.Body).ReadToEnd();
            var response = JsonSerializer.Deserialize<ExceptionResponse>(responseBody);
            Assert.That(response.Message, Is.EqualTo("Generic error message"));
        }
    }
}
