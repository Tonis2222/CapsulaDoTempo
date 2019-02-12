using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
  public class CapsulaDoTempoService
  {
    IRepositorioCapsulaDoTempo repositorio;

    public CapsulaDoTempoService(IRepositorioCapsulaDoTempo _repositorio)
    {
      repositorio = _repositorio;
    }
    
    public async Task<ResultadoBuscaCapsula> BuscarCapsulaPorId(string id)
    {
      var c = await repositorio.RecuperarCapsula(id);

      if (c == null)
        return new ResultadoBuscaCapsula() { ResultadoBusca = ResultadoBusca.NaoEncontrado };

      switch (c.Estado)
      {
        case EstadoCapsula.Aberta:
          return new ResultadoBuscaCapsula() { ResultadoBusca = ResultadoBusca.Encontrado, Capsula = c };

        case EstadoCapsula.Expirada:
          await repositorio.ExcluirCapsula(id);
          return new ResultadoBuscaCapsula() { ResultadoBusca = ResultadoBusca.NaoEncontrado };

        case EstadoCapsula.Criada:
        default:
          return new ResultadoBuscaCapsula() { ResultadoBusca = ResultadoBusca.CapsulaFechada };
      }
    }

    public async Task<ResultadoCriacaoCapsula> CriarCapsula(string id, CapsulaDoTempo capsula)
    {
      capsula.Id = id;
      capsula.DataCriacao = DateTime.UtcNow;
      capsula.DataAbertura = capsula.DataAbertura.ToUniversalTime();

      string msgErro;
      if (!capsula.ValidarCriacao(out msgErro))
      {
        return new ResultadoCriacaoCapsula() { ResultadoCriacao = ResultadoCriacao.NaoCriada, Mensagem = msgErro };
      }

      var c = await repositorio.RecuperarCapsula(capsula.Id);

      if (c == null)
      {
        await repositorio.CriarCapsula(capsula);
        return new ResultadoCriacaoCapsula() { ResultadoCriacao = ResultadoCriacao.Criada };
      }
      else if (c.Estado == EstadoCapsula.Expirada)
      {
        await repositorio.ExcluirCapsula(capsula.Id);
        await repositorio.CriarCapsula(capsula);
        return new ResultadoCriacaoCapsula() { ResultadoCriacao = ResultadoCriacao.Criada };

      }
      else
      {
        return new ResultadoCriacaoCapsula() { ResultadoCriacao = ResultadoCriacao.CapsulaJaExistente };
      }
    }
  }
}
