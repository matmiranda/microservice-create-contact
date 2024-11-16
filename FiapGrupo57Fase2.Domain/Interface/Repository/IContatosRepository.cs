using FiapGrupo57Fase2.Domain.Entity;
using FiapGrupo57Fase2.Domain.Enum;
using FiapGrupo57Fase2.DTO.Request;
using FiapGrupo57Fase2.DTO.Response;

namespace FiapGrupo57Fase2.Domain.Interface.Repository
{
    public interface IContatosRepository
    {
        Task<ContatosGetResponse> ObterContatoPorId(int id);
        Task<int> Adicionar(ContatoEntity contato);
        Task<bool> ContatoExiste(ContatosPostRequest contato);
        Task<bool> ContatoExistePorId(int id);
        List<ContatosGetResponse> ObterPorDDD(int ddd);
        List<ContatosGetResponse> ObterPorDDDRegiao(int ddd, RegiaoEnum regiao);
        Task Atualizar(ContatoEntity contato);
        Task Excluir(int id);
    }
}
