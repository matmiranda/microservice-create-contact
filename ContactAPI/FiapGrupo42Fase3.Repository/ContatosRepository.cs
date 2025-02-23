using FiapGrupo42Fase3.Domain.Entity;
using FiapGrupo42Fase3.Domain.Enum;
using FiapGrupo42Fase3.Domain.Interface.Dapper;
using FiapGrupo42Fase3.Domain.Interface.Repository;
using System.Data;

namespace FiapGrupo42Fase3.Repository
{
    public class ContatosRepository : IContatosRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDapperWrapper _dapperWrapper;

        public ContatosRepository(IDbConnection dbConnection, IDapperWrapper dapperWrapper)
        {
            _dbConnection = dbConnection;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<int> Adicionar(ContatoEntity contato)
        {
            var sql = @"INSERT INTO Contatos (Nome, Telefone, Email, DDD, Regiao) VALUES (@Nome, @Telefone, @Email, @DDD, @Regiao);SELECT LAST_INSERT_ID();";
            return await _dapperWrapper.QuerySingleAsync<int>(_dbConnection, sql, contato);
        }

        public async Task<bool> ContatoExistePorEmail(string email)
        {
            var sql = "SELECT 1 FROM Contatos WHERE Email = @Email";
            return await _dapperWrapper.QueryFirstOrDefaultAsync<int>(_dbConnection, sql, new { email }) > 0;
        }

        public async Task<bool> ContatoExistePorId(int id)
        {
            var sql = "SELECT 1 FROM Contatos WHERE Id = @Id";
            return await _dapperWrapper.QueryFirstOrDefaultAsync<int>(_dbConnection, sql, new { Id = id }) > 0;
        }

        public async Task<ContatoEntity> ObterContatoPorId(int id)
        {
            throw new System.NotImplementedException("Azure Functions");
            var sql = "SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.Id = @id";
            return await _dapperWrapper.QueryFirstOrDefaultAsync<ContatoEntity>(_dbConnection, sql, new { Id = id });
        }

        public async Task<IEnumerable<ContatoEntity>> ObterPorDDD(int ddd)
        {
            var sql = "SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.DDD = @DDD";
            return await _dapperWrapper.QueryAsync<ContatoEntity>(_dbConnection, sql, new { DDD = ddd });
        }

        public async Task<IEnumerable<ContatoEntity>> ObterPorDDDRegiao(int ddd, RegiaoEnum regiao)
        {
            var sql = @"SELECT c.Id, c.Nome, c.Telefone, c.Email, c.DDD, r.Nome AS Regiao FROM Contatos c JOIN Regioes r ON c.Regiao = r.Id WHERE c.DDD = @DDD AND c.Regiao = @Regiao";
            return await _dapperWrapper.QueryAsync<ContatoEntity>(_dbConnection, sql, new { DDD = ddd, Regiao = regiao });
        }

        public async Task Atualizar(ContatoEntity contato)
        {
            var sql = "UPDATE Contatos SET Nome = @Nome, Telefone = @Telefone, Email = @Email, DDD = @DDD, Regiao = @Regiao WHERE Id = @Id";
            await _dapperWrapper.ExecuteAsync(_dbConnection, sql, contato);
        }

        public async Task Excluir(int id)
        {
            var sql = "DELETE FROM Contatos WHERE Id = @Id";
            await _dapperWrapper.ExecuteAsync(_dbConnection, sql, new { Id = id });
        }
    }
}
