using FiapGrupo57Fase2.DTO.Request;
using FiapGrupo57Fase2.DTO.Response;

namespace FiapGrupo57Fase2.Domain.Interface.Service
{
    public interface IContatosService
    {
        Task<ContatosGetResponse> ObterContatoPorId(int id);
        Task<ContatosPostResponse> AdicionarContato(ContatosPostRequest contato);
        Task<List<ContatosGetResponse>> ObterContatos(int ddd);
        Task AtualizarContato(ContatosPutRequest contato);
        Task ExcluirContato(int id);
    }
}
