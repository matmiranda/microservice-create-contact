using FiapGrupo42Fase3.Domain.Constant;
using FiapGrupo42Fase3.Domain.Entity;

namespace FiapGrupo42Fase3.Domain.Message
{
    public class ContatoMessageMQ
    {
        public Guid MessageId { get; set; } // Identificador único da mensagem
        public DateTime DateTime { get; set; } // Data e hora da mensagem
        public string? Source { get; set; } // Origem da mensagem (ex: "API", "SistemaLegado")
        public ContatoEntity? Contato { get; set; } // Dados do contato
        public AcaoContatoConstant? Action { get; set; } // Ação a ser executada (ex: Create, Update, Delete)

        public ContatoMessageMQ()
        {
            MessageId = Guid.NewGuid(); // Gera um ID único para a mensagem
            DateTime = DateTime.UtcNow.AddHours(-3); // Define o timestamp atual
        }
    }
}
