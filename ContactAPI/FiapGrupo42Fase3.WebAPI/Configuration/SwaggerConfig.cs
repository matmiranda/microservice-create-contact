using FiapGrupo42Fase3.DTO.Request;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FiapGrupo42Fase3.WebAPI.Configuration
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Grupo 57 - Fase 1",
                    Version = "v1",
                    Description = "Uma API para gerenciar contatos regionais"
                });

                // Adicione exemplos diretamente na configuração do Swagger
                c.MapType<ContatosPostRequest>(() => new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["nome"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("João Silva")
                        },
                        ["telefone"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("123456789")
                        },
                        ["email"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("joao.silva@example.com")
                        },
                        ["ddd"] = new OpenApiSchema
                        {
                            Type = "integer",
                            Example = new OpenApiInteger(11)
                        }
                    }
                });
                c.MapType<ContatosPutRequest>(() => new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>
                    {
                        ["id"] = new OpenApiSchema
                        {
                            Type = "int",
                            Example = new OpenApiString("0")
                        },
                        ["nome"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("João Silva")
                        },
                        ["telefone"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("123456789")
                        },
                        ["email"] = new OpenApiSchema
                        {
                            Type = "string",
                            Example = new OpenApiString("joao.silva@example.com")
                        },
                        ["ddd"] = new OpenApiSchema
                        {
                            Type = "integer",
                            Example = new OpenApiInteger(11)
                        }
                    }
                });

                // Obter o caminho do arquivo XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1"));
        }
    }
}
