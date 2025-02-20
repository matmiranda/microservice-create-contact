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
builder.Services.AddSingleton<RabbitMQProducer>();



//// Adicionar serviços e repositórios
//builder.Services.AddScoped<IObterRegiaoPorDDD, ObterRegiaoPorDDD>();
//builder.Services.AddScoped<IContatosRepository, ContatosRepository>();
//builder.Services.AddScoped<IContatosService, ContatosService>();
//builder.Services.AddScoped<IDapperWrapper, DapperWrapper>();

// Adicionar RabbitMQ como Message Bus
//builder.Services.AddSingleton<IMessageBus, RabbitMQMessageBus>();


//// Configuração do Entity Framework DbContext
//builder.Services.AddDbContext<ContatoContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//    new MySqlServerVersion(new Version(8, 0, 23))));

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