﻿using CriarContatos.Domain.Models.RabbitMq;
using CriarContatos.Domain.Requests;
using CriarContatos.Infrastructure.Exceptions;
using CriarContatos.Service.Cadastro;
using CriarContatos.Service.RabbitMq;
using Moq;
using System.Net;

namespace CriarContatos.Test.Services
{
    [TestFixture]
    public class CadastroServiceTest
    {
        private Mock<IRabbitMqPublisherService> mockRabbitMqPublisherService;
        private CadastroService cadastroService;

        [SetUp]
        public void SetUp()
        {
            // Inicializa o mock do IRabbitMqPublisherService
            mockRabbitMqPublisherService = new Mock<IRabbitMqPublisherService>();
            cadastroService = new CadastroService(mockRabbitMqPublisherService.Object);
        }

        [Test]
        public async Task AdicionarContato_ShouldPublishMessage_WhenValidRequest()
        {
            // Arrange: Cria um objeto de request válido
            var cadastroRequest = new CadastroRequest
            {
                Nome = "Nome Teste",
                Telefone = "123456789",
                Email = "teste@teste.com",
                DDD = 11 // DDD válido
            };

            // Ação: Chama o método AdicionarContato
            await cadastroService.AdicionarContato(cadastroRequest);

            // Assert: Verifica se o método PublicarContatoAsync foi chamado uma vez
            mockRabbitMqPublisherService.Verify(service => service.PublicarContatoAsync(It.IsAny<ContactMessage>()), Times.Once);
        }

        [Test]
        public void ObtemRegiaoPorDDD_ShouldThrowException_WhenDDDIsInvalid()
        {
            // Arrange: DDD inválido
            int dddInvalido = 999;

            // Ação & Assert: Verifica se uma exceção é lançada com a mensagem esperada
            var exception = Assert.Throws<CustomException>(() => CadastroService.ObtemRegiaoPorDDD(dddInvalido));
            Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(exception.Message, Is.EqualTo("Região NÃO ENCONTRADA para o DDD: 999"));
        }

        [Test]
        public void ObtemRegiaoPorDDD_ShouldReturnRegion_WhenDDDIsValid()
        {
            // Arrange: DDD válido
            int dddValido = 11;

            // Ação: Chama o método ObtemRegiaoPorDDD
            var regiao = CadastroService.ObtemRegiaoPorDDD(dddValido);

            // Assert: Verifica se a região retornada está correta
            Assert.That(regiao, Is.EqualTo("4")); // Região 4 para o DDD 11
        }
    }
}
