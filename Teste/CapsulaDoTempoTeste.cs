using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace Teste
{
  public class CapsulaDoTempoTeste
  {
    [Fact]
    public void CriarCapsulaTeste()
    {
      var mockRepositorio = new Moq.Mock<DomainModel.Interfaces.Repositories.IRepositorioCapsulaDoTempo>();
      var mockNotificador = new Moq.Mock<DomainModel.Interfaces.INotificacaoService>();
      var mockCrypto = new Moq.Mock<DomainModel.Interfaces.Services.ICryptoService>();

      var capsulaDoTempoService = new DomainService.CapsulaDoTempoService(mockRepositorio.Object, mockNotificador.Object, mockCrypto.Object);
      
      var idCapsula = "CapsulaTeste";
      var capsula = new DomainModel.Entities.CapsulaDoTempo()
      {
        DataAbertura = DateTime.Now.AddDays(1),
        Duracao = DuracaoCapsula.UmAno,
        Mensagem = "teste",
        Email = "email@teste.com"

      };

      var act1 = capsulaDoTempoService.CriarCapsula(idCapsula, capsula);
      act1.Wait();
      var capsulacriada = act1.Result;
      Assert.True(capsulacriada.ResultadoCriacao == DomainService.ResultadoCriacao.Criada);

      mockRepositorio.Verify(a => a.CriarCapsula(Moq.It.IsAny<CapsulaDoTempo>()), Moq.Times.Once);
    }
    [Fact]
    public void EditarCapsulaTeste()
    {


      var mockRepositorio = new RepositorioCapsulaParaTeste();
      var mockNotificador = new Moq.Mock<DomainModel.Interfaces.INotificacaoService>();
      var mockCrypto = new Moq.Mock<DomainModel.Interfaces.Services.ICryptoService>();

      var capsulaDoTempoService = new DomainService.CapsulaDoTempoService(mockRepositorio, mockNotificador.Object, mockCrypto.Object);

      var idCapsula = "CapsulaTeste";
      var capsula = new DomainModel.Entities.CapsulaDoTempo()
      {
        DataAbertura = DateTime.Now.AddDays(1),
        Duracao = DuracaoCapsula.UmAno,
        Mensagem = "teste",
        Email = "email@teste.com"

      };

      var act1 = capsulaDoTempoService.CriarCapsula(idCapsula, capsula);
      act1.Wait();
      var capsulacriada = act1.Result;
      Assert.True(capsulacriada.ResultadoCriacao == DomainService.ResultadoCriacao.Criada);

      var act2 = mockRepositorio.RecuperarCapsula(idCapsula);
      act2.Wait();
      var capsulaBuscada1 = act2.Result;

      var act3 = capsulaDoTempoService.BuscarCapsulaParaEdicao(idCapsula, capsulaBuscada1.ChaveCapsula);
      act3.Wait();
      var capsulaBuscadaParaEdicao = act3.Result;

      Assert.True(capsulaBuscadaParaEdicao.ResultadoBusca == DomainService.ResultadoBusca.Encontrado);
      Assert.True(capsulaBuscadaParaEdicao.Capsula.Editavel);

      capsulaBuscadaParaEdicao.Capsula.DataAbertura = DateTime.Today.AddYears(2);
      var act4 = capsulaDoTempoService.AlterarCapsulaDoTempo(idCapsula, capsulaBuscadaParaEdicao.Capsula);
      act4.Wait();
      var resultadoAlteracaoCapsula = act4.Result;

      Assert.True(resultadoAlteracaoCapsula == DomainService.ResultadoAlteracao.Alterada);

      var act5 = capsulaDoTempoService.BuscarCapsulaParaEdicao(idCapsula, capsulaBuscada1.ChaveCapsula);
      act5.Wait();
      var capsulaAlteradaFinal = act5.Result;

      Assert.NotNull(capsulaAlteradaFinal.Capsula);
      Assert.True(capsulaAlteradaFinal.Capsula.DataAbertura.Date == DateTime.Today.AddYears(2));

    }
    [Fact]
    public void ExcluirCapsulaTeste()
    {


      var mockRepositorio = new RepositorioCapsulaParaTeste();
      var mockNotificador = new Moq.Mock<DomainModel.Interfaces.INotificacaoService>();
      var mockCrypto = new Moq.Mock<DomainModel.Interfaces.Services.ICryptoService>();

      var capsulaDoTempoService = new DomainService.CapsulaDoTempoService(mockRepositorio, mockNotificador.Object, mockCrypto.Object);

      var idCapsula = "CapsulaTeste";
      var capsula = new DomainModel.Entities.CapsulaDoTempo()
      {
        DataAbertura = DateTime.Now.AddDays(1),
        Duracao = DuracaoCapsula.UmAno,
        Mensagem = "teste",
        Email = "email@teste.com"

      };

      var act1 = capsulaDoTempoService.CriarCapsula(idCapsula, capsula);
      act1.Wait();
      var capsulacriada = act1.Result;
      Assert.True(capsulacriada.ResultadoCriacao == DomainService.ResultadoCriacao.Criada);

      var act2 = mockRepositorio.RecuperarCapsula(idCapsula);
      act2.Wait();
      var capsulaBuscada1 = act2.Result;

      var act3 = capsulaDoTempoService.BuscarCapsulaParaEdicao(idCapsula, capsulaBuscada1.ChaveCapsula);
      act3.Wait();
      var capsulaBuscadaParaEdicao = act3.Result;

      Assert.True(capsulaBuscadaParaEdicao.ResultadoBusca == DomainService.ResultadoBusca.Encontrado);
      Assert.True(capsulaBuscadaParaEdicao.Capsula.Editavel);

      capsulaBuscadaParaEdicao.Capsula.DataAbertura = DateTime.Today.AddYears(2);
      var act4 = capsulaDoTempoService.ExcluirCapsuladoTempo(idCapsula, capsulaBuscadaParaEdicao.Capsula.ChaveCapsula);
      act4.Wait();
      var resultadoAlteracaoCapsula = act4.Result;

      Assert.True(resultadoAlteracaoCapsula == DomainService.ResultadoExclusao.Excluida);

      var act5 = capsulaDoTempoService.BuscarCapsulaParaEdicao(idCapsula, capsulaBuscada1.ChaveCapsula);
      act5.Wait();
      var capsulaAlteradaFinal = act5.Result;

      Assert.Null(capsulaAlteradaFinal.Capsula);
      Assert.True(capsulaAlteradaFinal.ResultadoBusca == DomainService.ResultadoBusca.NaoEncontrado);

    }
    [Fact]
    public void BuscarCapsulaAbertaTeste()
    {
      var mockRepositorio = new RepositorioCapsulaParaTeste();
      var mockNotificador = new Moq.Mock<DomainModel.Interfaces.INotificacaoService>();
      var mockCrypto = new Moq.Mock<DomainModel.Interfaces.Services.ICryptoService>();

      var capsulaDoTempoService = new DomainService.CapsulaDoTempoService(mockRepositorio, mockNotificador.Object, mockCrypto.Object);

      var idCapsula = "CapsulaTeste";
      var capsula = new DomainModel.Entities.CapsulaDoTempo()
      {
        DataAbertura = DateTime.Now.AddSeconds(2),
        Duracao = DuracaoCapsula.UmAno,
        Mensagem = "teste",
        Email = "email@teste.com"

      };

      var act1 = capsulaDoTempoService.CriarCapsula(idCapsula, capsula);
      act1.Wait();
      var capsulacriada = act1.Result;
      Assert.True(capsulacriada.ResultadoCriacao == DomainService.ResultadoCriacao.Criada);

      System.Threading.Thread.Sleep(2000);
      var act2 = mockRepositorio.RecuperarCapsula(idCapsula);
      act2.Wait();
      var capsulaBuscada1 = act2.Result;

      Assert.True(capsulaBuscada1.Estado == EstadoCapsula.Aberta);

    }
    [Fact]
    public void BuscarCapsulaFechadaTeste()
    {
      var mockRepositorio = new RepositorioCapsulaParaTeste();
      var mockNotificador = new Moq.Mock<DomainModel.Interfaces.INotificacaoService>();
      var mockCrypto = new Moq.Mock<DomainModel.Interfaces.Services.ICryptoService>();

      var capsulaDoTempoService = new DomainService.CapsulaDoTempoService(mockRepositorio, mockNotificador.Object, mockCrypto.Object);

      var idCapsula = "CapsulaTeste";
      var capsula = new DomainModel.Entities.CapsulaDoTempo()
      {
        DataAbertura = DateTime.Now.AddDays(2),
        Duracao = DuracaoCapsula.UmAno,
        Mensagem = "teste",
        Email = "email@teste.com"

      };

      var act1 = capsulaDoTempoService.CriarCapsula(idCapsula, capsula);
      act1.Wait();
      var capsulacriada = act1.Result;
      Assert.True(capsulacriada.ResultadoCriacao == DomainService.ResultadoCriacao.Criada);

      var act2 = mockRepositorio.RecuperarCapsula(idCapsula);
      act2.Wait();
      var capsulaBuscada1 = act2.Result;

      Assert.True(capsulaBuscada1.Estado == EstadoCapsula.Criada);

    }



  }
  public class RepositorioCapsulaParaTeste : DomainModel.Interfaces.Repositories.IRepositorioCapsulaDoTempo
  {
    List<CapsulaDoTempo> listaCapsulaDoTempo = new List<CapsulaDoTempo>();

    public async Task AtualizarCapsula(CapsulaDoTempo capsula)
    {
      listaCapsulaDoTempo.RemoveAll(a => a.Id == capsula.Id);
      listaCapsulaDoTempo.Add(capsula);
    }

    public async Task CriarCapsula(CapsulaDoTempo capsula)
    {
      listaCapsulaDoTempo.Add(capsula);
    }

    public async Task ExcluirCapsula(string id)
    {
      listaCapsulaDoTempo.RemoveAll(a => a.Id == id);
    }

    public async Task<CapsulaDoTempo> RecuperarCapsula(string id)
    {
      return listaCapsulaDoTempo.FirstOrDefault(a => a.Id == id);
    }

    public async Task<List<CapsulaDoTempo>> RecuperarCapsulasAbertasOuExpiradas()
    {
      return listaCapsulaDoTempo.Where(a => a.DataAbertura < DateTime.Now).ToList();
    }
  }
}
