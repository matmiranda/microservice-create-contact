using CriarContatos.Api.Controllers;
using CriarContatos.Domain.Requests;
using CriarContatos.Infrastructure.Exceptions;
using CriarContatos.Service.Cadastro;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace CriarContatos.Test.Controllers
{
    [TestFixture]
    public class CadastroControllerTest
    {
        private Mock<ICadastroService> mockCadastroService;
        private CadastroController cadastroController;

        [SetUp]
        public void SetUp()
        {
            // Arrange: Inicializa o mock do ICadastroService
            mockCadastroService = new Mock<ICadastroService>();
            cadastroController = new CadastroController(mockCadastroService.Object);
        }

        [Test]
        public async Task PostContato_ShouldReturnAccepted_WhenValidRequest()
        {
            // Arrange: Cria um objeto de request válido
            var cadastroRequest = new CadastroRequest
            {
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com",
                DDD = 11
            };

            // Ação: Simula o comportamento do método AdicionarContato
            mockCadastroService.Setup(service => service.AdicionarContato(It.IsAny<CadastroRequest>()))
                               .Returns(Task.CompletedTask);

            // Ação: Chama o método PostContato
            var result = await cadastroController.PostContato(cadastroRequest);

            // Assert: Verifica se o resultado é do tipo ActionResult e contém um AcceptedResult
            Assert.That(result.Result, Is.InstanceOf<AcceptedResult>());
        }

        [Test]
        public void PostContato_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange: Cria um objeto de request válido
            var cadastroRequest = new CadastroRequest
            {
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com",
                DDD = 11
            };

            // Ação: Simula uma exceção no serviço
            mockCadastroService.Setup(service => service.AdicionarContato(It.IsAny<CadastroRequest>()))
                               .ThrowsAsync(new CustomException(HttpStatusCode.Conflict, "Contato com este email já existe."));

            // Ação: Chama o método PostContato
            var ex = Assert.ThrowsAsync<CustomException>(async () => await cadastroController.PostContato(cadastroRequest));

            // Assert: Verifica a mensagem da exceção
            Assert.That(ex.Message, Is.EqualTo("Contato com este email já existe."));
        }
    }
}
