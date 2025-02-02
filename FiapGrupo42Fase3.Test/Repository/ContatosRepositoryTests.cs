using FiapGrupo42Fase3.Domain.Entity;
using FiapGrupo42Fase3.Domain.Enum;
using FiapGrupo42Fase3.Domain.Interface.Dapper;
using FiapGrupo42Fase3.Repository;
using Moq;
using System.Data;

namespace FiapGrupo42Fase3.Test.Repository
{
    [TestFixture]
    public class ContatosRepositoryTests
    {
        private Mock<IDbConnection> _dbConnectionMock;
        private Mock<IDapperWrapper> _dapperWrapperMock;
        private ContatosRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _dbConnectionMock = new Mock<IDbConnection>();
            _dapperWrapperMock = new Mock<IDapperWrapper>();
            _repository = new ContatosRepository(_dbConnectionMock.Object, _dapperWrapperMock.Object);
        }

        [Test]
        public async Task Adicionar_ContatoValido_RetornaId()
        {
            // Arrange
            var contato = new ContatoEntity { Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = RegiaoEnum.Sudeste };
            var sql = @"INSERT INTO Contatos (Nome, Telefone, Email, DDD, Regiao) VALUES (@Nome, @Telefone, @Email, @DDD, @Regiao);SELECT LAST_INSERT_ID();";
            _dapperWrapperMock.Setup(d => d.QuerySingleAsync<int>(_dbConnectionMock.Object, sql, contato)).ReturnsAsync(1);

            // Act
            var result = await _repository.Adicionar(contato);

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task ContatoExiste_ContatoExistente_RetornaTrue()
        {
            // Arrange
            var email = "joao.silva@example.com";
            var sql = "SELECT 1 FROM Contatos WHERE Email = @Email";
            _dapperWrapperMock.Setup(d => d.QueryFirstOrDefaultAsync<int>(_dbConnectionMock.Object, sql, It.IsAny<object>())).ReturnsAsync(1);

            // Act
            var result = await _repository.ContatoExistePorEmail(email);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ContatoExistePorId_ContatoExistente_RetornaTrue()
        {
            // Arrange
            var id = 1;
            var sql = "SELECT 1 FROM Contatos WHERE Id = @Id";
            _dapperWrapperMock.Setup(d => d.QueryFirstOrDefaultAsync<int>(_dbConnectionMock.Object, sql, It.IsAny<object>())).ReturnsAsync(1);

            // Act
            var result = await _repository.ContatoExistePorId(id);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ObterPorDDD_ContatosExistentes_RetornaListaDeContatos()
        {
            // Arrange
            var ddd = 11;
            var sql = "SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.DDD = @DDD";
            var contatos = new List<ContatoEntity>
            {
                new() { Id = 1, Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = RegiaoEnum.Sudeste }
            };

            // Configuração do mock para retornar a lista de ContatoEntity
            _dapperWrapperMock.Setup(d => d.QueryAsync<ContatoEntity>(_dbConnectionMock.Object, sql, It.IsAny<object>())).ReturnsAsync(contatos);

            // Act
            var result = await _repository.ObterPorDDD(ddd);

            // Assert
            Assert.That(result, Is.EqualTo(contatos));
        }

        [Test]
        public async Task ObterPorDDDRegiao_ContatosExistentes_RetornaListaDeContatos()
        {
            // Arrange
            var ddd = 11;
            RegiaoEnum regiao = RegiaoEnum.Sudeste;
            var sql = @"SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.DDD = @DDD AND c.Regiao = @Regiao";
            var contatos = new List<ContatoEntity>
            {
                new() { Id = 1, Nome = "João Silva", Telefone = "123456789", Email = "joao.silva@example.com", DDD = 11, Regiao = RegiaoEnum.Sudeste }
            };

            // Configuração do mock para retornar a lista de ContatoEntity
            _dapperWrapperMock.Setup(d => d.QueryAsync<ContatoEntity>(_dbConnectionMock.Object, sql, It.IsAny<object>())).ReturnsAsync(contatos);

            // Act
            var result = await _repository.ObterPorDDDRegiao(ddd, regiao);

            // Assert
            Assert.That(result, Is.EqualTo(contatos));
        }

        [Test]
        public async Task ObterContatoPorId_ContatoExistente_RetornaContato()
        {
            // Arrange
            var id = 1;
            var sql = "SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.Id = @id";
            var contato = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = RegiaoEnum.Sudeste
            };

            // Configuração do mock para retornar o ContatoEntity
            _dapperWrapperMock.Setup(d => d.QueryFirstOrDefaultAsync<ContatoEntity>(_dbConnectionMock.Object, sql, It.IsAny<object>())).ReturnsAsync(contato);

            // Act
            var result = await _repository.ObterContatoPorId(id);

            // Assert
            Assert.That(result, Is.EqualTo(contato));
        }

        [Test]
        public async Task Atualizar_ContatoExistente_ExecutaComandoSQL()
        {
            // Arrange
            var contato = new ContatoEntity
            {
                Id = 1,
                Nome = "João Silva",
                Telefone = "123456789",
                Email = "joao.silva@example.com",
                DDD = 11,
                Regiao = RegiaoEnum.Sudeste
            };
            var sql = "UPDATE Contatos SET Nome = @Nome, Telefone = @Telefone, Email = @Email, DDD = @DDD, Regiao = @Regiao WHERE Id = @Id";

            // Configuração do mock para simular a execução do comando SQL
            _dapperWrapperMock.Setup(d => d.ExecuteAsync(_dbConnectionMock.Object, sql, contato)).Returns(Task.CompletedTask);

            // Act
            await _repository.Atualizar(contato);

            // Assert
            _dapperWrapperMock.Verify(d => d.ExecuteAsync(_dbConnectionMock.Object, sql, contato), Times.Once);
        }

        [Test]
        public async Task Excluir_ContatoExistente_ExecutaComandoSQL()
        {
            // Arrange
            var id = 1;
            var sql = "DELETE FROM Contatos WHERE Id = @Id";

            // Configuração do mock para simular a execução do comando SQL
            _dapperWrapperMock
                .Setup(d => d.ExecuteAsync(_dbConnectionMock.Object, sql, It.Is<object>(param => (int)param.GetType().GetProperty("Id").GetValue(param, null) == id))).Returns(Task.CompletedTask);

            // Act
            await _repository.Excluir(id);

            // Assert
            _dapperWrapperMock.
                Verify(d => d.ExecuteAsync(_dbConnectionMock.Object, sql, It.Is<object>(param => (int)param.GetType().GetProperty("Id").GetValue(param, null) == id)), Times.Once);
        }
    }
}
