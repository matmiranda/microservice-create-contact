using FiapGrupo42Fase3.Domain.Entity;
using FiapGrupo42Fase3.Domain.Enum;
using FiapGrupo42Fase3.DTO.Request;
using FiapGrupo42Fase3.DTO.Response;

namespace FiapGrupo42Fase3.Service.Mapper
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

        public static ContatosGetResponse ContatoPorId(ContatoEntity contatoEntity)
        {
            return new ContatosGetResponse
            {
                Id = contatoEntity.Id,
                Nome = contatoEntity.Nome,
                Telefone = contatoEntity.Telefone,
                Email = contatoEntity.Email,
                DDD = contatoEntity.DDD,
                Regiao = contatoEntity.Regiao.ToString()
            };
        }

        public static IEnumerable<ContatosGetResponse> ContatosPorDDD(IEnumerable<ContatoEntity> contatos)
        {
            return contatos.Select(contatoEntity => new ContatosGetResponse
            {
                Id = contatoEntity.Id,
                Nome = contatoEntity.Nome,
                Telefone = contatoEntity.Telefone,
                Email = contatoEntity.Email,
                DDD = contatoEntity.DDD,
                Regiao = contatoEntity.Regiao.ToString()
            });
        }
    }
}
