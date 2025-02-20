using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using FiapGrupo42Fase3.DTO.Configuration;

public class RabbitMQProducer : IAsyncDisposable
{
    private readonly RabbitMQSettings _rabbitMQSettings;
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMQProducer(IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        _rabbitMQSettings = rabbitMQSettings.Value;
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.Host,
            UserName = _rabbitMQSettings.Username,
            Password = _rabbitMQSettings.Password
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task SendMessageAsync<T>(T message)
    {
        var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: _rabbitMQSettings.QueueName,
            body: messageBody
        );
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}
