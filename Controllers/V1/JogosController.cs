using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoServices _IJogoServices;

        public JogosController(IJogoServices iJogoServices)
        {
            _IJogoServices = iJogoServices;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está consultando, min. 1</param>
        /// <param name="quantidade">Indica quantidade de registros por pagina, min. 1 e max. 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Não há jogos</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var result = await _IJogoServices.Obter(pagina, quantidade);

            if (result == null)
                return NoContent();

            return Ok(result);
        }

        /// <summary>
        /// Retornar jogo por ID
        /// </summary>
        /// <param name="idJogo">ID do jogo</param>
        /// <response code="200">Retorna jogo filtrado</response>
        /// <response code="204">Não há jogo com este ID</response>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _IJogoServices.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Inserir([FromBody]JogoInputModel jogoInput)
        {
            try
            {
                var jogo = await _IJogoServices.Inserir(jogoInput);
                return Ok();
            }
            catch (JogoJaCadastrado ex)
            {
 
                return NotFound(ex);
            }

        }

        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> Atualizar([FromRoute]Guid idJogo, [FromBody]JogoInputModel jogo)
        {
            try
            {
                await _IJogoServices.Atualizar(idJogo, jogo);
                return Ok();
            }
            catch (JogoJaCadastrado ex)
            {
                return NotFound("Jogo já cadastrado!");
            }
            
        }

        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> Atualizar([FromRoute]Guid idJogo, [FromRoute]double preco)
        {
            try
            {
                await _IJogoServices.Atualizar(idJogo, preco);
                return Ok();
            }
            catch (JogoJaCadastrado ex)
            {
                return NotFound("Jogo não cadastrado!");             
            }
        }

        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> Deletar([FromRoute]Guid idJogo)
        {
            try
            {
                await _IJogoServices.Remover(idJogo);
                return Ok();
            }
            catch (JogoNaoCadastrado ex)
            {
                //return NotFound("Jogo não cadastrado!");
                return NotFound(ex.GetBaseException().ToString());
            }
        }

    }
}
