using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapsulaDoTempoUI.Models
{
  public class CapsulaDoTempoViewModel
  {
    public string Id { get; set; }
    public DateTime DataCriacao { get; set; }

    [DataType(DataType.Date)]
    public DateTime DataAbertura { get; set; }
    public string Mensagem { get; set; }

    [DataType(DataType.Upload)]
    public string Imagem { get; set; }
  }
}
