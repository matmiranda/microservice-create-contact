using FiapGrupo42Fase3.Domain.Constant;
using FiapGrupo42Fase3.Domain.Entity;

namespace FiapGrupo42Fase3.Domain.Message
{
    public class ContatoMessageMQ
    {
        public ContatoEntity? Contato { get; set; } // Dados do contato
        public AcaoContatoConstant? Action { get; set; } // Ação a ser executada (ex: Create, Update, Delete)
    }
}
