using DomainModel.Entities;
using System;
using Xunit;

namespace Teste
{
  public class CapsulaDoTempoTeste
  {
    [Fact]
    public void TestaCalculoDataExpiracao()
    {

      DateTime agora = DateTime.Now;
      DateTime dataCriacao = agora.AddHours(-2);
      DateTime dataAbertura = agora;
      DateTime dataExpiracaoEsperada = agora.AddHours(2);

      CapsulaDoTempo c = new CapsulaDoTempo();
      c.DataCriacao = dataCriacao;
      c.DataAbertura = dataAbertura;
      Assert.Equal(c.CalcularDataExpiracao(), dataExpiracaoEsperada);
    }
  }
}
