using FiapGrupo57Fase2.Domain.Entity;
using FiapGrupo57Fase2.Domain.Enum;
using FiapGrupo57Fase2.DTO.Request;
using FiapGrupo57Fase2.DTO.Response;

namespace FiapGrupo57Fase2.Domain.Interface.Repository
{
    public interface IContatosRepository
    {
        Task<ContatoEntity> ObterContatoPorId(int id);
        Task<int> Adicionar(ContatoEntity contato);
        Task<bool> ContatoExistePorEmail(string email);
        Task<bool> ContatoExistePorId(int id);
        Task<IEnumerable<ContatoEntity>> ObterPorDDD(int ddd);
        Task<IEnumerable<ContatoEntity>> ObterPorDDDRegiao(int ddd, RegiaoEnum regiao);
        Task Atualizar(ContatoEntity contato);
        Task Excluir(int id);
    }
}
