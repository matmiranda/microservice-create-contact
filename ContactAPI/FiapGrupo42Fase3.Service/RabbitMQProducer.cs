using FiapGrupo42Fase3.Domain.Interface.Service;
using FiapGrupo42Fase3.DTO.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMQProducer : IRabbitMQProducer, IDisposable
{
    private readonly RabbitMQSettings _rabbitMQSettings;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RabbitMQProducer(IOptions<RabbitMQSettings> rabbitMQSettings)
    {
        _rabbitMQSettings = rabbitMQSettings.Value;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public async Task InitializeAsync()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.Host,
            UserName = _rabbitMQSettings.Username,
            Password = _rabbitMQSettings.Password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: _rabbitMQSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
    }

    public async Task SendMessageAsync<T>(T message)
    {
        if (_channel == null)
        {
            throw new InvalidOperationException("O canal não foi inicializado.");
        }

        try
        {
            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, _jsonSerializerOptions));

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: _rabbitMQSettings.QueueName,
                body: messageBody
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
        }

        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
        GC.SuppressFinalize(this);
    }
}
