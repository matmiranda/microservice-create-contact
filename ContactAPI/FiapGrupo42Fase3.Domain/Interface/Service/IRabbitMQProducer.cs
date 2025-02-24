namespace FiapGrupo42Fase3.Domain.Interface.Service
{
    public interface IRabbitMQProducer : IAsyncDisposable
    {
        Task SendMessageAsync<T>(T message) where T : class;
    }
}
