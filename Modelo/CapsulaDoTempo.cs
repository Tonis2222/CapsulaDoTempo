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
    public DuracaoCapsula Duracao { set; get; }

    public EstadoCapsula CalcularEstadoCapsula()
    {
      if (DateTime.UtcNow < DataAbertura)
        return EstadoCapsula.Criada;
      else if (DateTime.UtcNow < CalcularDataExpiracao())
        return EstadoCapsula.Aberta;
      else
        return EstadoCapsula.Expirada;
    }

    public DateTime CalcularDataExpiracao()
    {

      switch (Duracao)
      {
        case DuracaoCapsula.UmDia:
          return DataAbertura.AddDays(1);
        case DuracaoCapsula.UmaSemana:
          return DataAbertura.AddDays(7);
        case DuracaoCapsula.UmMes:
          return DataAbertura.AddMonths(1);
        case DuracaoCapsula.UmAno:
          return DataAbertura.AddYears(1);
        default:
          throw new ArgumentOutOfRangeException("Duracao");
      }
    }

    public bool ValidarCriacao(out string msgErro)
    {
      msgErro = string.Empty;

      if (string.IsNullOrEmpty(Id))
      {
        msgErro = "Id não informado.";
        return false;
      }

      if (DataAbertura < DateTime.UtcNow)
      {
        msgErro = "A data de abertura deve uma data futura.";
        return false;
      }

      if (Mensagem?.Length > 1000)
      {
        msgErro = "A mensagem deve ter no máximo 1000 caracteres";
        return false;
      }

      if (!string.IsNullOrEmpty(Imagem) && CalculaTamanhoBytes(Imagem) > 512000)
      {
        msgErro = "A Imagem não deve ter mais de 512 KB";
        return false;
      }
      
      return true;
    }

    private int CalculaTamanhoBytes(string imagem)
    {
      return 2 * (imagem.Length / 3);
    }
  }
}
