using FiapGrupo42Fase3.Domain;
using FiapGrupo42Fase3.Domain.Constant;
using FiapGrupo42Fase3.Domain.Entity;
using FiapGrupo42Fase3.Domain.Enum;
using FiapGrupo42Fase3.Domain.Interface;
using FiapGrupo42Fase3.Domain.Interface.Repository;
using FiapGrupo42Fase3.Domain.Interface.Service;
using FiapGrupo42Fase3.Domain.Message;
using FiapGrupo42Fase3.DTO.Request;
using FiapGrupo42Fase3.DTO.Response;
using FiapGrupo42Fase3.Infrastructure.Exception;
using FiapGrupo42Fase3.Service.Helper;
using FiapGrupo42Fase3.Service.Mapper;
using System.Net;

namespace FiapGrupo42Fase3.Service
{
    public class ContatosService : IContatosService
    {
        private readonly IContatosRepository _contatosRepository;
        private readonly IRabbitMQProducer _rabbitMQProducer;
        private readonly IObterRegiaoPorDDD _obterRegiaoPorDDD;

        public ContatosService(IRabbitMQProducer rabbitMQProducer, IObterRegiaoPorDDD obterRegiaoPorDDD)
        {
            _rabbitMQProducer = rabbitMQProducer;
            _obterRegiaoPorDDD = obterRegiaoPorDDD;
        }

        public async Task<ContatosGetResponse> ObterContatoPorId(int id)
        {
            //valida contato
            await ValidaIdContato(id);

            //consulta contato
            var contatoEntity =  await _contatosRepository.ObterContatoPorId(id);

            //mapper contato
            return ContatoMapper.ContatoPorId(contatoEntity);
        }

        public async Task<List<ContatosGetResponse>> ObterContatos(int ddd)
        {
            // Determina a região com base no DDD
            string regiaoPorDDD = _obterRegiaoPorDDD.ObtemRegiaoPorDDD(ddd);

            if (regiaoPorDDD.Equals("DDD_INVALIDO"))
                throw new CustomException(HttpStatusCode.BadRequest, $"Região não encontrada para o DDD: {ddd}");

            //// Consulta contatos por DDD e região determinada
            //var contatos = await _contatosRepository.ObterPorDDDRegiao(ddd, (RegiaoEnum)Enum.Parse(typeof(RegiaoEnum), regiaoPorDDD));
            //criar método para consultar no Azure Functions
            IEnumerable<ContatoEntity> contatos = Enumerable.Empty<ContatoEntity>();
            throw new NotImplementedException("ObterPorDDDRegiao - Azure Functions");

            if (!contatos.Any())
                throw new CustomException(HttpStatusCode.NotFound, "Contato não encontrado");

            // Mapper contatos
            return ContatoMapper.ContatosPorDDD(contatos).ToList();
        }
        public async Task<ContatosPostResponse> AdicionarContato(ContatosPostRequest contato)
        {
            ValidatorHelper.Validar(contato);

            //if (await _contatosRepository.ContatoExistePorEmail(contato.Email))
            //    throw new CustomException(HttpStatusCode.Conflict, "Contato com este email já existe.");
            //criar método para consultar no Azure Functions
            throw new NotImplementedException("ContatoExistePorEmail - Azure Functions");

            string regiao = _obterRegiaoPorDDD.ObtemRegiaoPorDDD(contato.DDD);

            if (regiao.Equals("DDD_INVALIDO"))
            {
                throw new CustomException(HttpStatusCode.BadRequest, $"Região NÃO ENCONTRADA para o DDD: {contato.DDD}");
            }
            contato.Regiao = regiao;

            ContatoEntity mapper = ContatoMapper.ToEntity(contato);

            int id = await _contatosRepository.Adicionar(mapper);

            return new ContatosPostResponse { Id = id };
        }

        public async Task AtualizarContato(ContatosPutRequest contato)
        {
            ValidatorHelper.Validar(contato);

            await ValidaIdContato(contato.Id);

            //var contatoAux = await _contatosRepository.ObterContatoPorId(contato.Id);
            //criar método para consultar no Azure Functions
            ContatoEntity? contatoAux = null;
            throw new NotImplementedException("ObterContatoPorId - Azure Functions");


            if (contatoAux.DDD != contato.DDD)
            {
                string regiao = _obterRegiaoPorDDD.ObtemRegiaoPorDDD(contato.DDD);
                if (regiao.Equals("DDD_INVALIDO"))
                {
                    throw new CustomException(HttpStatusCode.BadRequest, $"Região NÃO ENCONTRADA para o DDD: {contato.DDD}");
                }
                contato.Regiao = regiao;
            }

            ContatoEntity mapper = ContatoMapper.ToEntity(contato);

            await _contatosRepository.Atualizar(mapper);
        }
        public async Task ExcluirContato(int id)
        {
            await ValidaIdContato(id);

            var message = new ContatoMessageMQ
            {        
                Action = AcaoContatoConstant.Delete,
                Source = "API.ContatosService.ExcluirContato",
                Contato = new ContatoEntity { Id = id }
            };

            await _rabbitMQProducer.SendMessageAsync(message);
            //await _contatosRepository.Excluir(id);
        }

        private async Task ValidaIdContato(int id)
        {
            if (id == 0)
                throw new CustomException(HttpStatusCode.BadRequest, "O id deve ser maior que zero.");

            //no código abaixo, precisamos criar o método para consultar no Azure Function
            //if (!await _contatosRepository.ContatoExistePorId(id))
            //    throw new CustomException(HttpStatusCode.NotFound, $"O id do contato não existe.");
            throw new NotImplementedException("ContatoExistePorId - Azure Functions");
        }
    }
}
