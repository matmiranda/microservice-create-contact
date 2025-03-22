using System.Text.Json.Serialization;

namespace CriarContatos.Domain.Responses
{
    public class ContatoDuplicidade
    {
        [JsonPropertyName("isDuplicate")]
        public bool IsDuplicate { get; set; }
    }
}
