using System.Data;

namespace FiapGrupo57Fase2.Domain.Interface.Dapper
{
    public interface IDapperWrapper
    {
        Task<int> QuerySingleAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null);
        Task ExecuteAsync(IDbConnection connection, string sql, object param = null);
    }
}
