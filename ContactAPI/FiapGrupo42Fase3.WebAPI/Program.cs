using FiapGrupo42Fase3.Domain.Interface;
using FiapGrupo42Fase3.Domain.Interface.Dapper;
using FiapGrupo42Fase3.Domain.Interface.Repository;
using FiapGrupo42Fase3.Domain.Interface.Service;
using FiapGrupo42Fase3.Infrastructure.Data;
using FiapGrupo42Fase3.Infrastructure.Exception;
using FiapGrupo42Fase3.Repository;
using FiapGrupo42Fase3.Service;
using FiapGrupo42Fase3.WebAPI.Configuration;
using System.Data;
using Prometheus;
using FiapGrupo42Fase3.DTO.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Adicionar RabbitMQ como conexão persistente
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();


// Adicionar serviços e repositórios
builder.Services.AddScoped<IObterRegiaoPorDDD, ObterRegiaoPorDDD>();
builder.Services.AddScoped<IContatosService, ContatosService>();

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Inicializa o RabbitMQProducer
var rabbitMQProducer = app.Services.GetRequiredService<IRabbitMQProducer>();
await rabbitMQProducer.InitializeAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicionar middleware do Prometheus
app.UseMetricServer();
app.UseHttpMetrics();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();

//teste commit git hub action