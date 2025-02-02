using FiapGrupo42Fase3.DTO.Request;
using FiapGrupo42Fase3.DTO.Response;

namespace FiapGrupo42Fase3.Domain.Interface.Service
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
