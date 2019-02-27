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

      if (!string.IsNullOrEmpty(capsula.Email))
      {
        capsula.ChaveCapsula = CriptoService.Criptografar(capsula.Id,capsula.Email);
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

    public async Task<ResultadoAlteracao> AlterarCapsulaDoTempo(string id, CapsulaDoTempo capsula)
    {
      var c = await repositorio.RecuperarCapsula(id);

      if (c == null)
      {
        return ResultadoAlteracao.CapsulaNaoEncontrada;
      }
      else
      {
        if (!c.Editavel)
        {
          return ResultadoAlteracao.CapsulaNaoPermiteEdicao;
        }
        else if (c.ChaveCapsula == capsula.ChaveCapsula)
        {
          c.DataAbertura = capsula.DataAbertura;
          c.Duracao = capsula.Duracao;
          c.Imagem = capsula.Imagem;
          c.Mensagem = capsula.Mensagem;

          string retornoValidacao;
          if (!c.ValidarCriacao(out retornoValidacao))
          {
            return ResultadoAlteracao.CapsulaNaoValidada;
          }
          
          await repositorio.AtualizarCapsula(c);
          return ResultadoAlteracao.Alterada;
        }
        else
        {
          return ResultadoAlteracao.ChaveInvalida;
        }
      }
    }

    public async Task<ResultadoExclusao> ExcluirCapsuladoTempo(string id, string chaveCapsula)
    {
      var c = await repositorio.RecuperarCapsula(id);

      if (c == null)
      {
        return ResultadoExclusao.CapsulaNaoEncontrada;
      }
      else
      {
        if (!c.Editavel)
        {
          return ResultadoExclusao.CapsulaNaoPermiteEdicao;
        }
        else if (c.ChaveCapsula == chaveCapsula)
        {
          await repositorio.ExcluirCapsula(id);
          return ResultadoExclusao.Excluida;
        }
        else
        {
          return ResultadoExclusao.ChaveInvalida;
        }
      }
    }
  }
}
