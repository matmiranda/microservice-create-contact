using CriarContatos.Domain.Enum;
using CriarContatos.Domain.Models.RabbitMq;
using CriarContatos.Domain.Requests;

namespace CriarContatos.Service.Mapper
{
    public static class ContatoMapper
    {
        public static ContactMessage ToContactMessage(CadastroRequest request, string regiao)
        {
            return new ContactMessage
            {
                Nome = request.Nome,
                Telefone = request.Telefone,
                Email = request.Email,
                DDD = request.DDD,
                Regiao = (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), regiao),
                CreatedAt = DateTime.UtcNow.AddHours(-3)
            };
        }
    }
}
