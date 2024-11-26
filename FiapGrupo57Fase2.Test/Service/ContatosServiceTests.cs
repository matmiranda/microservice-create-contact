using FiapGrupo57Fase2.Domain.Entity;
using FiapGrupo57Fase2.Domain.Enum;
using FiapGrupo57Fase2.Domain.Interface;
using FiapGrupo57Fase2.Domain.Interface.Repository;
using FiapGrupo57Fase2.DTO.Request;
using FiapGrupo57Fase2.DTO.Response;
using FiapGrupo57Fase2.Infrastructure.Exception;
using FiapGrupo57Fase2.Service;
using FluentAssertions;
using Moq;
using System.Net;

namespace FiapGrupo57Fase2.Test.Service
{
    [TestFixture]
    public class ContatosServiceTests
    {
        private Mock<IContatosRepository> _mockContatosRepository;
        private Mock<IObterRegiaoPorDDD> _mockObterRegiaoPorDDD;
        private ContatosService _contatosService;

        [SetUp]
        public void Setup()
        {
            _mockContatosRepository = new Mock<IContatosRepository>();
            _mockObterRegiaoPorDDD = new Mock<IObterRegiaoPorDDD>();
            _contatosService = new ContatosService(_mockContatosRepository.Object, _mockObterRegiaoPorDDD.Object);
        }

        [Test]
        public async Task ObterContatos_DDDValido_DeveRetornarContatos()
        {
            // Arrange
            int ddd = 11;
            string regiao = "Sudeste";
            var contatos = new List<ContatoEntity>
        {
            new ContatoEntity { Id = 1, Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = RegiaoEnum.Sudeste }
        };

            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(ddd)).Returns(regiao);
            _mockContatosRepository.Setup(repo => repo.ObterPorDDDRegiao(ddd, RegiaoEnum.Sudeste)).ReturnsAsync(contatos);

            // Act
            var result = await _contatosService.ObterContatos(ddd);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Nome, Is.EqualTo("João Silva"));
        }

        [Test]
        public async Task ObterContatoPorId_IdValido_DeveRetornarContato()
        {
            // Arrange
            var contatoId = 1;
            var contatoEntity = new ContatoEntity { Id = contatoId, Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = RegiaoEnum.Sudeste };
            var contatoEsperado = new ContatosGetResponse { Id = contatoId, Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = "Sudeste" };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contatoId))
                .ReturnsAsync(true);
            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contatoId))
                .ReturnsAsync(contatoEntity);

            // Act
            var resultado = await _contatosService.ObterContatoPorId(contatoId);

            // Assert
            resultado.Should().BeEquivalentTo(contatoEsperado);
        }

        [Test]
        public void ObterContatos_DDDInvalido_DeveLancarCustomException()
        {
            // Arrange
            var ddd = 99;
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(ddd))
                .Returns("DDD_INVALIDO");

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.ObterContatos(ddd));
            Assert.Multiple(() =>
            {
                Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Message, Is.EqualTo($"Região não encontrada para o DDD: {ddd}"));
            });
            _mockObterRegiaoPorDDD.Verify(service => service.ObtemRegiaoPorDDD(ddd), Times.Once);
            _mockContatosRepository.Verify(repo => repo.ObterPorDDDRegiao(It.IsAny<int>(), It.IsAny<RegiaoEnum>()), Times.Never);
        }

        [Test]
        public void ObterContatos_ContatosNaoEncontrados_DeveLancarCustomException()
        {
            // Arrange
            var ddd = 11;
            var regiao = "Sudeste";
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(ddd))
                .Returns(regiao);
            _mockContatosRepository.Setup(repo => repo.ObterPorDDDRegiao(ddd, RegiaoEnum.Sudeste))
                .ReturnsAsync(new List<ContatoEntity>());

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.ObterContatos(ddd));
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(ex.Message, Is.EqualTo("Contato não encontrado"));
            _mockObterRegiaoPorDDD.Verify(service => service.ObtemRegiaoPorDDD(ddd), Times.Once);
            _mockContatosRepository.Verify(repo => repo.ObterPorDDDRegiao(ddd, RegiaoEnum.Sudeste), Times.Once);
        }

        [Test]
        public async Task AdicionarContato_ContatoValido_DeveAdicionarContato()
        {
            // Arrange
            var contatoRequest = new ContatosPostRequest
            {
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11
            };
            var contatoEntity = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = RegiaoEnum.Sudeste
            };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorEmail(contatoRequest.Email))
                .ReturnsAsync(false);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contatoRequest.DDD))
                .Returns("Sudeste");
            _mockContatosRepository.Setup(repo => repo.Adicionar(It.IsAny<ContatoEntity>()))
                .ReturnsAsync(contatoEntity.Id);

            // Act
            var resultado = await _contatosService.AdicionarContato(contatoRequest);

            // Assert
            Assert.That(resultado.Id, Is.EqualTo(contatoEntity.Id));
            _mockContatosRepository.Verify(repo => repo.ContatoExistePorEmail(contatoRequest.Email), Times.Once);
            _mockObterRegiaoPorDDD.Verify(service => service.ObtemRegiaoPorDDD(contatoRequest.DDD), Times.Once);
            _mockContatosRepository.Verify(repo => repo.Adicionar(It.IsAny<ContatoEntity>()), Times.Once);
        }

        [Test]
        public void AdicionarContato_DDDInvalido_DeveLancarCustomException()
        {
            // Arrange
            var contatoRequest = new ContatosPostRequest
            {
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 99
            };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorEmail(contatoRequest.Email))
                .ReturnsAsync(false);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contatoRequest.DDD))
                .Returns("DDD_INVALIDO");

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AdicionarContato(contatoRequest));
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(ex.Message, Is.EqualTo($"Região NÃO ENCONTRADA para o DDD: {contatoRequest.DDD}"));
            _mockContatosRepository.Verify(repo => repo.ContatoExistePorEmail(contatoRequest.Email), Times.Once);
            _mockObterRegiaoPorDDD.Verify(service => service.ObtemRegiaoPorDDD(contatoRequest.DDD), Times.Once);
            _mockContatosRepository.Verify(repo => repo.Adicionar(It.IsAny<ContatoEntity>()), Times.Never);
        }

        [Test]
        public void AdicionarContato_EmailExistente_DeveLancarCustomException()
        {
            // Arrange
            var contatoRequest = new ContatosPostRequest
            {
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11
            };
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorEmail(contatoRequest.Email))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = Assert.ThrowsAsync<CustomException>(() => _contatosService.AdicionarContato(contatoRequest));
            Assert.That(ex.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(ex.Message, Is.EqualTo("Contato com este email já existe."));
            _mockContatosRepository.Verify(repo => repo.ContatoExistePorEmail(contatoRequest.Email), Times.Once);
            _mockObterRegiaoPorDDD.Verify(service => service.ObtemRegiaoPorDDD(It.IsAny<int>()), Times.Never);
            _mockContatosRepository.Verify(repo => repo.Adicionar(It.IsAny<ContatoEntity>()), Times.Never);
        }

        [Test]
        public async Task AtualizarContato_ContatoValido_DeveAtualizarContato()
        {
            // Arrange
            var contatoRequest = new ContatosPutRequest
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = "Sudeste"
            };
            var contatoEntity = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 21,
                Regiao = RegiaoEnum.Sudeste
            };

            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contatoRequest.Id))
                .ReturnsAsync(contatoEntity);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contatoRequest.DDD))
                .Returns("Sudeste");
            _mockContatosRepository.Setup(repo => repo.Atualizar(It.IsAny<ContatoEntity>()))
                .Returns(Task.CompletedTask);
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contatoRequest.Id)).ReturnsAsync(true);

            // Act
            await _contatosService.AtualizarContato(contatoRequest);

            // Assert
            _mockContatosRepository.Verify(repo => repo.ObterContatoPorId(contatoRequest.Id), Times.Once);
            _mockObterRegiaoPorDDD.Verify(service => service.ObtemRegiaoPorDDD(contatoRequest.DDD), Times.Once);
            _mockContatosRepository.Verify(repo => repo.Atualizar(It.IsAny<ContatoEntity>()), Times.Once);
        }

        [Test]
        public void AtualizarContato_DDDInvalido_DeveLancarExcecao()
        {
            // Arrange
            var contatoRequest = new ContatosPutRequest
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 99,
                Regiao = "Sudeste"
            };
            var contatoEntity = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 21,
                Regiao = RegiaoEnum.Sudeste
            };

            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contatoRequest.Id))
                .ReturnsAsync(contatoEntity);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contatoRequest.DDD))
                .Returns("DDD_INVALIDO");
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contatoRequest.Id)).ReturnsAsync(true);

            // Act & Assert
            var exception = Assert.ThrowsAsync<CustomException>(async () => await _contatosService.AtualizarContato(contatoRequest));

            Assert.Multiple(() =>
            {
                Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(exception.Message, Is.EqualTo($"Região NÃO ENCONTRADA para o DDD: {contatoRequest.DDD}"));
            });
        }

        [Test]
        public async Task AtualizarContato_ShouldUpdateContact_WhenValid()
        {
            // Arrange
            var contato = new ContatosPutRequest
            {
                Id = 1,
                DDD = 11,
                Regiao = "Sudeste",
                Email = "joao.silva@example.com",
                Nome = "João Silva",
                Telefone = "123456789"
            };
            var contatoAux = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = RegiaoEnum.Sudeste
            };

            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contato.Id)).ReturnsAsync(contatoAux);
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contato.Id)).ReturnsAsync(true);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contato.DDD)).Returns("SP");

            // Act
            await _contatosService.AtualizarContato(contato);

            // Assert
            _mockContatosRepository.Verify(repo => repo.Atualizar(It.Is<ContatoEntity>(c => c.DDD == 11 && c.Regiao == RegiaoEnum.Sudeste)), Times.Once);
        }

        [Test]
        public void AtualizarContato_ShouldThrowException_WhenDDDInvalid()
        {
            // Arrange
            var contato = new ContatosPutRequest
            {
                Id = 1,
                DDD = 99,
                Regiao = "Sudeste",
                Email = "joao.silva@example.com",
                Nome = "João Silva",
                Telefone = "123456789"
            };
            var contatoAux = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 21,
                Regiao = RegiaoEnum.Sudeste
            };

            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contato.Id)).ReturnsAsync(contatoAux);
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(contato.Id)).ReturnsAsync(true);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contato.DDD)).Returns("DDD_INVALIDO");

            // Act & Assert
            var exception = Assert.ThrowsAsync<CustomException>(async () => await _contatosService.AtualizarContato(contato));

            Assert.Multiple(() =>
            {
                Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(exception.Message, Is.EqualTo($"Região NÃO ENCONTRADA para o DDD: {contato.DDD}"));
            });
        }

        [Test]
        public void AtualizarContato_ShouldThrowException_WhenIDInvalid()
        {
            // Arrange
            var contato = new ContatosPutRequest
            {
                Id = 1,
                DDD = 21,
                Regiao = "Sudeste",
                Email = "joao.silva@example.com",
                Nome = "João Silva",
                Telefone = "123456789"
            };
            var contatoAux = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 21,
                Regiao = RegiaoEnum.Sudeste
            };

            _mockContatosRepository.Setup(repo => repo.ObterContatoPorId(contato.Id)).ReturnsAsync(contatoAux);
            _mockObterRegiaoPorDDD.Setup(service => service.ObtemRegiaoPorDDD(contato.DDD)).Returns("DDD_INVALIDO");

            // Act & Assert
            var exception = Assert.ThrowsAsync<CustomException>(async () => await _contatosService.AtualizarContato(contato));

            Assert.Multiple(() =>
            {
                Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(exception.Message, Is.EqualTo("O id do contato não existe."));
            });
        }

        [Test]
        public void AtualizarContato_ShouldThrowException_WhenIdIsZero()
        {
            // Arrange
            var contato = new ContatosPutRequest
            {
                Id = 0,
                DDD = 21,
                Regiao = "Sudeste",
                Email = "joao.silva@example.com",
                Nome = "João Silva",
                Telefone = "123456789"
            };

            // Act & Assert
            var exception = Assert.ThrowsAsync<CustomException>(async () => await _contatosService.AtualizarContato(contato));
            Assert.Multiple(() =>
            {
                Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(exception.Message, Is.EqualTo("O id deve ser maior que zero."));
            });
        }

        [Test]
        public async Task ExcluirContato_ShouldCallRepository_WhenIdIsValid()
        {
            // Arrange
            int validId = 1;
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(validId)).ReturnsAsync(true);

            // Act
            await _contatosService.ExcluirContato(validId);

            // Assert
            _mockContatosRepository.Verify(repo => repo.Excluir(validId), Times.Once);
        }

        [Test]
        public void ExcluirContato_ShouldThrowException_WhenIdIsZero()
        {
            // Arrange
            int invalidId = 0;

            // Act & Assert
            var exception = Assert.ThrowsAsync<CustomException>(async () => await _contatosService.ExcluirContato(invalidId));
            Assert.Multiple(() =>
            {
                Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(exception.Message, Is.EqualTo("O id deve ser maior que zero."));
            });
        }

        [Test]
        public void ExcluirContato_ShouldThrowException_WhenIdDoesNotExist()
        {
            // Arrange
            int nonExistentId = 99;
            _mockContatosRepository.Setup(repo => repo.ContatoExistePorId(nonExistentId)).ReturnsAsync(false);

            // Act & Assert
            var exception = Assert.ThrowsAsync<CustomException>(async () => await _contatosService.ExcluirContato(nonExistentId));
            Assert.Multiple(() =>
            {
                Assert.That(exception.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(exception.Message, Is.EqualTo($"O id do contato não existe."));
            });
        }
    }
}
