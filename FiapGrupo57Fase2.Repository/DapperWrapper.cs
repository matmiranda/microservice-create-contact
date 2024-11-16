using Dapper;
using FiapGrupo57Fase2.Domain.Interface.Dapper;
using System.Data;

namespace FiapGrupo57Fase2.Repository
{
    public class DapperWrapper : IDapperWrapper
    {
        public async Task<int> QuerySingleAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return await connection.QuerySingleAsync<int>(sql, param);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public IEnumerable<T> Query<T>(IDbConnection connection, string sql, object param = null)
        {
            return connection.Query<T>(sql, param);
        }

        public async Task ExecuteAsync(IDbConnection connection, string sql, object param = null)
        {
            await connection.ExecuteAsync(sql, param);
        }
    }
}
