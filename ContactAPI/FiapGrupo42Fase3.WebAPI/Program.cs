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
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Configuração do MassTransit - RabbitMQSettings
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

// Configuração do MassTransit
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMQSettings = context.GetRequiredService<IOptions<RabbitMQSettings>>().Value;

        cfg.Host(new Uri(rabbitMQSettings.Host), h =>
        {
            h.Username(rabbitMQSettings.Username);
            h.Password(rabbitMQSettings.Password);
        });
    });
});

builder.Services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();

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