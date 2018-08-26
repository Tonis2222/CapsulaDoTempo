using System.ComponentModel.DataAnnotations;

namespace CapsulaDoTempoUI.Models
{
  public enum DuracaoCapsula
  {
    [Display(Name = "Um Dia")]
    UmDia = 1,
    [Display(Name = "Uma Semana")]
    UmaSemana = 2,
    [Display(Name = "Um Mês")]
    UmMes = 3,
    [Display(Name = "Um Ano")]
    UmAno = 4,
  }
}