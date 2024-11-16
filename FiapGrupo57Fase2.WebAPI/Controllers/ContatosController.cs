using FiapGrupo57Fase2.Domain.Interface.Service;
using FiapGrupo57Fase2.DTO.Request;
using FiapGrupo57Fase2.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace FiapGrupo57Fase2.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContatosController : Controller
    {
        private readonly IContatosService _contatoService;

        public ContatosController(IContatosService contatoService)
        {
            _contatoService = contatoService;
        }

        /// <summary>
        /// Obtém um contato pelo ID.
        /// </summary>
        /// <param name="id">O ID do contato.</param>
        /// <returns>O contato correspondente ao ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(404, "Contato não encontrado", typeof(ExceptionResponse), "application/json")]
        public async Task<ActionResult<ContatosGetResponse>> GetContatoById(int id)
        {
            return Ok(await _contatoService.ObterContatoPorId(id));
        }

        /// <summary>
        /// Obtém uma lista de contatos filtrados pelo DDD.
        /// </summary>
        /// <param name="ddd">O DDD da região.</param>
        /// <param name="regiao">A região opcional para filtrar os contatos.</param>
        /// <returns>Uma lista de contatos.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(400, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        public ActionResult<IEnumerable<ContatosGetResponse>> GetContatos(
            [FromQuery, BindRequired] int ddd,
            [FromQuery] string? regiao = null)
        {
            return Ok(_contatoService.ObterContatos(ddd, regiao));
        }

        /// <summary>
        /// Adiciona um novo contato.
        /// </summary>
        /// <param name="contato">Os dados do contato a ser adicionado.</param>
        /// <returns>O contato adicionado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerResponse(400, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        [SwaggerResponse(409, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        public async Task<ActionResult<ContatosPostResponse>> PostContato(
            ContatosPostRequest contato)
        {
            return Ok(await _contatoService.AdicionarContato(contato));
        }

        /// <summary>
        /// Atualiza um contato existente.
        /// </summary>
        /// <param name="contato">Os dados do contato a ser atualizado.</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(400, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        [SwaggerResponse(404, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        public async Task<IActionResult> PutContato(
            ContatosPutRequest contato)
        {
            await _contatoService.AtualizarContato(contato);
            return NoContent();
        }

        /// <summary>
        /// Exclui um contato pelo id.
        /// </summary>
        /// <param name="id">O id do contato a ser excluído.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(400, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        [SwaggerResponse(404, "Erro de validação", typeof(ExceptionResponse), "application/json")]
        public async Task<IActionResult> DeleteContato(int id)
        {
            await _contatoService.ExcluirContato(id);
            return NoContent();
        }
    }
}
