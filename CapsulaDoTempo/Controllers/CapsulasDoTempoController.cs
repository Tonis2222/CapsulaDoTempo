using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Modelo;

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

      if (id == "ping")
        return new OkObjectResult("No ar!");

      var c = await repositorio.RecuperarCapsula(id);

      if (c == null)
        return NotFound();

      var e = c.CalcularEstadoCapsula();

      //apenas em DEV
      //e = EstadoCapsula.Aberta;

      switch (e)
      {         
        case EstadoCapsula.Aberta:
          return new OkObjectResult(c);

        case EstadoCapsula.Expirada:
          await repositorio.ExcluirCapsula(id);
          return NotFound();

        case EstadoCapsula.Criada:
        default:
          return Unauthorized();          
      }
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> Post(string id, [FromBody]Modelo.CapsulaDoTempo capsula)
    {

      capsula.Id = id;
      capsula.DataCriacao = DateTime.UtcNow;
      capsula.DataAbertura = capsula.DataAbertura.ToUniversalTime();

      string msgErro;
      if (!capsula.ValidarCriacao(out msgErro))
      {
        return BadRequest(msgErro);
      }

      var c = await repositorio.RecuperarCapsula(capsula.Id);

      if (c == null)
      {
        await repositorio.SalvarCapsula(capsula);
        return Ok();
      }
      else if (c.CalcularEstadoCapsula() == EstadoCapsula.Expirada)
      {
        await repositorio.ExcluirCapsula(capsula.Id);
        await repositorio.SalvarCapsula(capsula);
        return Ok();

      }
      else
      {
        return Unauthorized();
      }
    }
  }
}
