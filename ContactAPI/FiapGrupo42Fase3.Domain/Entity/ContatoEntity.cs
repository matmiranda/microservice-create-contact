using FiapGrupo42Fase3.Domain.Enum;
using System.Text.Json.Serialization;

namespace FiapGrupo42Fase3.Domain.Entity
{
    public class ContatoEntity
    {
        public int Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Nome { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Telefone { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Email { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int DDD { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RegiaoEnum? Regiao { get; set; }
    }
}
