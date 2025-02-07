using FiapGrupo42Fase3.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace FiapGrupo42Fase3.Infrastructure.Data
{
    public class ContatoContext : DbContext
    {
        public ContatoContext(DbContextOptions<ContatoContext> options) : base(options) { }

        public DbSet<ContatosPostRequest> Contatos { get; set; }

    }
}
