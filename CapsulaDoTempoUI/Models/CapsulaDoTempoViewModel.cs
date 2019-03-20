using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CapsulaDoTempoUI.Models
{
  public class CapsulaDoTempoViewModel
  {
    [Display(Name = "Data de Abertura")]
    [DataType(DataType.Date)]
    public DateTime DataAbertura { get; set; }

    [Display(Name = "Duração da Capsula")]
    public DuracaoCapsula Duracao { get; set; }

    public string Mensagem { get; set; }

    [DataType(DataType.Upload)]
    public IFormFile Imagem { get; set; }

    public string ImagemStr { get; set; }

    [Display(Name = "Email(opcional, permite modificar a capsula)")]
    public string Email { get; set; }

    public string Chave { get; set; }
  }
}
