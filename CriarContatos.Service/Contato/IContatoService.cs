using CriarContatos.Domain.Requests;

namespace CriarContatos.Service.Contato
{
    public interface IContatoService
    {
        Task AdicionarContato(ContatoRequest contato);
    }
}
