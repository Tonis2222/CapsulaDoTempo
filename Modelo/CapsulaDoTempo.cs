using System;

namespace Modelo
{
  public class CapsulaDoTempo
  {
    public string Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAbertura { get; set; }
    public string Mensagem { get; set; }
    public string Imagem { get; set; }

    public EstadoCapsula CalcularEstadoCapsula()
    {
      if (DateTime.Now < DataAbertura)
        return EstadoCapsula.Criada;
      else if (DateTime.Now < CalcularDataExpiracao())
        return EstadoCapsula.Aberta;
      else
        return EstadoCapsula.Expirada;
    }

    public DateTime CalcularDataExpiracao()
    {
      return DataAbertura + (DataAbertura - DataCriacao);
    }

  }
}
