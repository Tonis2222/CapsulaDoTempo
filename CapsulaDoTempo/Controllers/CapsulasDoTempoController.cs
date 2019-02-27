using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainModel.Interfaces.Repositories;
using DomainService;
using Microsoft.AspNetCore.Mvc;

namespace CapsulaDoTempo.Controllers
{
  [Route("api/[controller]")]
  public class CapsulasDoTempoController : Controller
  {
    IRepositorioCapsulaDoTempo repositorio;

    public CapsulasDoTempoController(IRepositorioCapsulaDoTempo _repositorio)
    {
      repositorio = _repositorio;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(string id)
    {
      var ds = new CapsulaDoTempoService(repositorio);

      var resultado = await ds.BuscarCapsulaPorId(id);

      switch (resultado.ResultadoBusca)
      {
        case ResultadoBusca.NaoEncontrado:
          return NotFound();
        case ResultadoBusca.Encontrado:
          return new OkObjectResult(resultado.Capsula);
        case ResultadoBusca.CapsulaFechada:
        default:
          return Unauthorized();
      }
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> Post(string id, [FromBody]DomainModel.Entities.CapsulaDoTempo capsula)
    {

      var ds = new CapsulaDoTempoService(repositorio);
      var resultado = await ds.CriarCapsula(id, capsula);

      switch (resultado.ResultadoCriacao)
      {
        case ResultadoCriacao.NaoCriada:
          return BadRequest(resultado.Mensagem);
        case ResultadoCriacao.Criada:
          return Ok();
        case ResultadoCriacao.CapsulaJaExistente:
        default:
          return Unauthorized();
      }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, [FromBody]DomainModel.Entities.CapsulaDoTempo capsula)
    {

      var ds = new CapsulaDoTempoService(repositorio);
      var resultado = await ds.AlterarCapsulaDoTempo(id, capsula);

      switch (resultado)
      {
        case ResultadoAlteracao.Alterada:
          return Ok();
        case ResultadoAlteracao.CapsulaNaoEncontrada:
          return NotFound();
        default:
          return Unauthorized();
      }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id, [FromBody]DomainModel.Entities.CapsulaDoTempo capsula)
    {
      var ds = new CapsulaDoTempoService(repositorio);

      var resultado = await ds.ExcluirCapsuladoTempo(id,capsula.ChaveCapsula);
      
      switch (resultado)
      {
        case ResultadoExclusao.Excluida:
          return Ok();
        case ResultadoExclusao.CapsulaNaoEncontrada:
          return NotFound(); 
        default:
          return Unauthorized();
      }
    }
  }
}
