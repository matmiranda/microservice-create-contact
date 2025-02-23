using FiapGrupo42Fase3.Domain.Converter;
using System.Text.Json.Serialization;

namespace FiapGrupo42Fase3.Domain.Constant
{
    [JsonConverter(typeof(AcaoContatoConstantConverter))]
    public record AcaoContatoConstant(string Value)
    {
        public static readonly AcaoContatoConstant Create = new("CREATE");
        public static readonly AcaoContatoConstant Update = new("UPDATE");
        public static readonly AcaoContatoConstant Delete = new("DELETE");

        public override string ToString() => Value;
    }
}
