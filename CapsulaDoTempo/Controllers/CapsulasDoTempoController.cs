using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainModel.Interfaces;
using DomainModel.Interfaces.Repositories;
using DomainModel.Interfaces.Services;
using DomainService;
using Microsoft.AspNetCore.Mvc;

namespace CapsulaDoTempo.Controllers
{
  [Route("api/[controller]")]
  public class CapsulasDoTempoController : Controller
  {
    IRepositorioCapsulaDoTempo repositorio;
    INotificacaoService notificacao;
    ICryptoService cryptoService;

    public CapsulasDoTempoController(IRepositorioCapsulaDoTempo _repositorio, INotificacaoService _notificacao, ICryptoService _cryptoService)
    {
      repositorio = _repositorio;
      notificacao = _notificacao;
      cryptoService = _cryptoService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Get(string id, string chave = null)
    {
      var ds = new CapsulaDoTempoService(repositorio,notificacao, cryptoService);

      var resultado = new ResultadoBuscaCapsula();

      var edicao = !string.IsNullOrEmpty(chave);
      
      if (edicao)
      {
        var chaveDecoded = System.Net.WebUtility.UrlDecode(chave).Replace(" ", "+");
        resultado = await ds.BuscarCapsulaParaEdicao(id, chaveDecoded);
      }
      else
      {
        resultado = await ds.BuscarCapsulaPorId(id);        
      }

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

      var ds = new CapsulaDoTempoService(repositorio, notificacao, cryptoService);
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

      var ds = new CapsulaDoTempoService(repositorio, notificacao, cryptoService);
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
      var ds = new CapsulaDoTempoService(repositorio, notificacao, cryptoService);

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
