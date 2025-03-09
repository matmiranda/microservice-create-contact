using CriarContatos.Domain.Requests;
using CriarContatos.Domain.Responses;
using CriarContatos.Service.Cadastro;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CriarContatos.Api.Controllers
{
    /// <summary>
    /// Controller responsável pelo cadastro de contatos.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CadastroController : Controller
    {
        private readonly ICadastroService cadastroService;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="CadastroController"/>.
        /// </summary>
        /// <param name="cadastroService">O serviço de cadastro de contatos.</param>
        public CadastroController(ICadastroService cadastroService)
        {
            this.cadastroService = cadastroService;
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
        public async Task<ActionResult<CadastroResponse>> PostContato(CadastroRequest contato)
        {
            await cadastroService.AdicionarContato(contato);
            return Accepted();
        }
    }
}
