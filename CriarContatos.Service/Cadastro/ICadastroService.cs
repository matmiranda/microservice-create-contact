using CriarContatos.Domain.Requests;

namespace CriarContatos.Service.Cadastro
{
    public interface ICadastroService
    {
        Task AdicionarContato(CadastroRequest contato);
    }
}
