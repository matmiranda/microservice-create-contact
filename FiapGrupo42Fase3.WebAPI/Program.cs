using FiapGrupo42Fase3.Domain.Interface;
using FiapGrupo42Fase3.Domain.Interface.Dapper;
using FiapGrupo42Fase3.Domain.Interface.Repository;
using FiapGrupo42Fase3.Domain.Interface.Service;
using FiapGrupo42Fase3.Infrastructure.Data;
using FiapGrupo42Fase3.Infrastructure.Exception;
using FiapGrupo42Fase3.Repository;
using FiapGrupo42Fase3.Service;
using FiapGrupo42Fase3.WebAPI.Configuration;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços e repositórios
builder.Services.AddTransient<IDbConnection>((sp) =>
    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IObterRegiaoPorDDD, ObterRegiaoPorDDD>();
builder.Services.AddScoped<IContatosRepository, ContatosRepository>();
builder.Services.AddScoped<IContatosService, ContatosService>();
builder.Services.AddScoped<IDapperWrapper, DapperWrapper>();

// Configuração do Entity Framework DbContext
builder.Services.AddDbContext<ContatoContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 23))));

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