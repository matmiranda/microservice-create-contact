using CriarContatos.Domain.Models.RabbitMq;
using CriarContatos.Domain.Requests;
using CriarContatos.Domain.Responses;
using CriarContatos.Infrastructure.Exceptions;
using CriarContatos.Service.Mapper;
using CriarContatos.Service.RabbitMq;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CriarContatos.Service.Contato
{
    public class ContatoService : IContatoService
    {
        private readonly IRabbitMqPublisherService _rabbitMqPublisherService;
        private readonly HttpClient _httpClient;
        private readonly string _key;

        public ContatoService(IRabbitMqPublisherService rabbitMqPublisherService, HttpClient httpClient, IConfiguration configuration)
        {
            _rabbitMqPublisherService = rabbitMqPublisherService;
            _httpClient = httpClient;
            _key = configuration["ApiAzure:Key"] ?? throw new CustomException(HttpStatusCode.InternalServerError, "ApiAzure:Key configuration is missing.");
        }

        public async Task AdicionarContato(ContatoRequest contato)
        {
            if (await ValidaDuplicidade(contato.Email))
                throw new CustomException(HttpStatusCode.Conflict, "Contato com este email já existe.");

            string regiao = ObtemRegiaoPorDDD(contato.DDD);

            ContactMessage contactMessage = ContatoMapper.ToContactMessage(contato, regiao);

            // Enviar para a fila do RabbitMQ
            await _rabbitMqPublisherService.PublicarContatoAsync(contactMessage);
        }

        public static string ObtemRegiaoPorDDD(int DDD)
        {
            string regiao = DDD switch
            {
                63 or 68 or 69 or 92 or 95 or 96 or 97 => "1",
                71 or 73 or 74 or 75 or 77 or 79 or 81 or 82 or 83 or 84 or 85 or 86 or 87 or 88 or 89 or 98 or 99 => "2",
                61 or 62 or 64 or 65 or 66 or 67 => "3",
                11 or 12 or 13 or 14 or 15 or 16 or 17 or 18 or 19 or 21 or 22 or 24 or 27 or 28 or 31 or 32 or 33 or 34 or 35 or 37 or 38 => "4",
                41 or 42 or 43 or 44 or 45 or 46 or 47 or 48 or 49 or 51 or 53 or 54 or 55 => "5",
                _ => $"DDD_INVALIDO",
            };

            if (regiao.Equals("DDD_INVALIDO"))
                throw new CustomException(HttpStatusCode.BadRequest, $"Região NÃO ENCONTRADA para o DDD: {DDD}");

            return regiao;
        }

        public async Task<bool> ValidaDuplicidade(string email)
        {
            string url = "https://fiap-api-gateway.azure-api.net/contato/valida-duplicidade";

            if (!_httpClient.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))            
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);
            

            var content = new StringContent(JsonSerializer.Serialize(new { email }), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new CustomException(HttpStatusCode.InternalServerError, $"Erro ao consultar Api Azure: {response.ReasonPhrase}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var contatoDuplicidade = JsonSerializer.Deserialize<ContatoDuplicidade>(responseContent);

            if (contatoDuplicidade == null)            
                throw new CustomException(HttpStatusCode.InternalServerError, "Resposta da Api Azure está vazia ou inválida.");
            

            return contatoDuplicidade.IsDuplicate;
        }
    }
}
