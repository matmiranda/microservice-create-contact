using FiapGrupo42Fase3.Domain.Entity;
using FiapGrupo42Fase3.Domain.Enum;
using FiapGrupo42Fase3.DTO.Request;
using FiapGrupo42Fase3.DTO.Response;

namespace FiapGrupo42Fase3.Domain.Interface.Repository
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
