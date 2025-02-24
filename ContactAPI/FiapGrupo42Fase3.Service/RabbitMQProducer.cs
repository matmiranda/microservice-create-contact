using FiapGrupo42Fase3.Domain.Interface.Service;
using FiapGrupo42Fase3.DTO.Configuration;
using MassTransit;
using Microsoft.Extensions.Options;

public class RabbitMQProducer : IRabbitMQProducer, IDisposable, IAsyncDisposable
{
    private readonly RabbitMQSettings _rabbitMQSettings;
    private readonly IBusControl _busControl;

    public RabbitMQProducer(IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        _rabbitMQSettings = rabbitMQSettings.Value ?? throw new ArgumentNullException(nameof(rabbitMQSettings));

        // Configuração do MassTransit
        _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(new Uri(_rabbitMQSettings.Host), h =>
            {
                h.Username(_rabbitMQSettings.Username);
                h.Password(_rabbitMQSettings.Password);
            });
        });

        // Inicia o bus
        _busControl.Start();
    }

    public async Task SendMessageAsync<T>(T message) where T : class
    {
        if (_busControl == null)
        {
            throw new InvalidOperationException("O bus do MassTransit não foi inicializado.");
        }

        try
        {
            // Publica a mensagem no RabbitMQ
            var endpoint = await _busControl.GetSendEndpoint(new Uri($"queue:{_rabbitMQSettings.QueueName}"));
            await endpoint.Send(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _busControl?.Stop();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_busControl != null)
        {
            await _busControl.StopAsync();
        }
        GC.SuppressFinalize(this);
    }
}
