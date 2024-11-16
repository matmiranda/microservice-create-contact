using FiapGrupo57Fase2.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace FiapGrupo57Fase2.Infrastructure.Data
{
    public class ContatoContext : DbContext
    {
        public ContatoContext(DbContextOptions<ContatoContext> options) : base(options) { }

        public DbSet<ContatosPostRequest> Contatos { get; set; }

    }
}
