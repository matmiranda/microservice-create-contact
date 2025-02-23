namespace FiapGrupo42Fase3.Domain.Interface.Service
{
    public interface IRabbitMQProducer : IAsyncDisposable
    {
        Task InitializeAsync();
        Task SendMessageAsync<T>(T message);
    }
}
