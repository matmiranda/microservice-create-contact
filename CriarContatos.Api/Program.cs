using CriarContatos.Api.Swagger;
using CriarContatos.Infrastructure.Exceptions;
using CriarContatos.Service.Cadastro;
using Prometheus;
using CriarContatos.Infrastructure.MassTransit;
using CriarContatos.Service.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço de health check
builder.Services.AddHealthChecks();

// Adiciona a configuração do appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // Permite sobrescrever com variáveis de ambiente

// Configurar MassTransit
builder.Services.ConfigureMassTransit(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddScoped<ICadastroService, CadastroService>();

builder.Services.AddScoped<IRabbitMqPublisherService, RabbitMqPublisherService>();

var app = builder.Build();

// Mapeia o endpoint de health check
app.MapHealthChecks("/health");

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
