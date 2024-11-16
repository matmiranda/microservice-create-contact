using FiapGrupo57Fase2.DTO.Request;
using FiapGrupo57Fase2.DTO.Response;

namespace FiapGrupo57Fase2.Domain.Interface.Service
{
    public interface IContatosService
    {
        Task<ContatosGetResponse> ObterContatoPorId(int id);
        Task<ContatosPostResponse> AdicionarContato(ContatosPostRequest contato);
        List<ContatosGetResponse> ObterContatos(int ddd, string? regiao);
        Task AtualizarContato(ContatosPutRequest contato);
        Task ExcluirContato(int id);
    }
}
