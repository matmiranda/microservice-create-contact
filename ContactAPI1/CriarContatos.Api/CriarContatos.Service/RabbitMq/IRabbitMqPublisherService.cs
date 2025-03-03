using CriarContatos.Domain.Models.RabbitMq;

namespace CriarContatos.Service.RabbitMq
{
    public interface IRabbitMqPublisherService
    {
        Task PublicarContatoAsync(ContactMessage contactMessage);
    }
}
