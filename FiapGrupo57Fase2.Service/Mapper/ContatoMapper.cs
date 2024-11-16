using FiapGrupo57Fase2.Domain.Entity;
using FiapGrupo57Fase2.Domain.Enum;
using FiapGrupo57Fase2.DTO.Request;

namespace FiapGrupo57Fase2.Service.Mapper
{
    public static class ContatoMapper
    {
        public static ContatoEntity ToEntity(ContatosPostRequest request)
        {
            return new ContatoEntity
            {
                Nome = request.Nome,
                Telefone = request.Telefone,
                Email = request.Email,
                DDD = request.DDD,
                Regiao = (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), request.Regiao)
            };
        }

        public static ContatoEntity ToEntity(ContatosPutRequest request)
        {
            return new ContatoEntity
            {
                Id = request.Id,
                Nome = request.Nome,
                Telefone = request.Telefone,
                Email = request.Email,
                DDD = request.DDD,
                Regiao = (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), request.Regiao)
            };
        }
    }
}
