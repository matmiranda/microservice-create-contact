using CriarContatos.Domain.Models.RabbitMq;
using CriarContatos.Domain.Requests;
using CriarContatos.Infrastructure.Exceptions;
using CriarContatos.Service.Contato;
using CriarContatos.Service.RabbitMq;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CriarContatos.Test.Services
{
    [TestFixture]
    public class ContatoServiceTest : IDisposable
    {
        private Mock<IRabbitMqPublisherService> mockRabbitMqPublisherService;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;
        private ContatoService contatoService;

        [SetUp]
        public void SetUp()
        {
            mockRabbitMqPublisherService = new Mock<IRabbitMqPublisherService>();
            mockConfiguration = new Mock<IConfiguration>();

            // Configurando a chave da API simulada
            mockConfiguration.Setup(config => config["ApiAzure:Key"]).Returns("fake-key");

            // Mockando o HttpClient usando HttpMessageHandler
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://fiap-api-gateway.azure-api.net/")
            };

            contatoService = new ContatoService(mockRabbitMqPublisherService.Object, httpClient, mockConfiguration.Object);
        }

        [TearDown]
        public void TearDown()
        {
            httpClient.Dispose();
        }

        [Test]
        public async Task AdicionarContato_ShouldPublishMessage_WhenValidRequest()
        {
            // Arrange
            var contatoRequest = new ContatoRequest
            {
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com",
                DDD = 11 // DDD válido
            };

            // Mockando a resposta da API de validação de duplicidade
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new { IsDuplicate = false }), Encoding.UTF8, "application/json")
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockResponse);

            // Act
            await contatoService.AdicionarContato(contatoRequest);

            // Assert
            mockRabbitMqPublisherService.Verify(service => service.PublicarContatoAsync(It.IsAny<ContactMessage>()), Times.Once);
        }

        [Test]
        public void ObtemRegiaoPorDDD_ShouldThrowException_WhenDDDIsInvalid()
        {
            int dddInvalido = 999;

            var exception = Assert.Throws<CustomException>(() => ContatoService.ObtemRegiaoPorDDD(dddInvalido));
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo("Região NÃO ENCONTRADA para o DDD: 999"));
        }

        [Test]
        public void ObtemRegiaoPorDDD_ShouldReturnRegion_WhenDDDIsValid()
        {
            int dddValido = 11;

            var regiao = ContatoService.ObtemRegiaoPorDDD(dddValido);

            Assert.That(regiao, Is.EqualTo("4"));
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
