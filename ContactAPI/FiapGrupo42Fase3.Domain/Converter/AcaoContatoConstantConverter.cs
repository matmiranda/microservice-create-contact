using FiapGrupo42Fase3.Domain.Constant;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FiapGrupo42Fase3.Domain.Converter
{
    public class AcaoContatoConstantConverter : JsonConverter<AcaoContatoConstant>
    {
        public override AcaoContatoConstant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? value = reader.GetString() ?? throw new JsonException("Valor nulo para AcaoContatoConstant");

            return value switch
            {
                "CREATE" => AcaoContatoConstant.Create,
                "UPDATE" => AcaoContatoConstant.Update,
                "DELETE" => AcaoContatoConstant.Delete,
                _ => throw new JsonException($"Valor inválido para AcaoContatoConstant: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, AcaoContatoConstant value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}
