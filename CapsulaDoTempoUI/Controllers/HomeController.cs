using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CapsulaDoTempoUI.Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using System.Net;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace CapsulaDoTempoUI.Controllers
{
  public class HomeController : Controller
  {
    string urlApi = "";
    public HomeController(IConfiguration config)
    {
      urlApi = config.GetValue<string>("URLCapsulaDoTempoAPI");
    }

    [HttpGet]
    public IActionResult Index()
    {
      return View("Index");
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id,string chave = null)
    {
      if (string.IsNullOrWhiteSpace(id))
      {
        //Mostra o manual
        return Ok();
      }

      if (id == "ping")
      {
        return View("CapsulaExistente", new CapsulaDoTempoViewModel() { Mensagem = "exemplo" });
      }

      HttpClient cli = new HttpClient();

      var edicao = !string.IsNullOrEmpty(chave);

      var url = urlApi + id + (edicao ? ("?chave=" + chave) : string.Empty);

      var result = await cli.GetAsync(new Uri(url));

      if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      {
        return View("CapsulaFechada");
      }
      else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        return View("NovaCapsula");
      }
      else if (result.StatusCode == System.Net.HttpStatusCode.OK)
      {
        var bdResult = await result.Content.ReadAsStringAsync();
        dynamic obj = Newtonsoft.Json.Linq.JObject.Parse(bdResult);
        if (edicao)
        {
          return View("EditarCapsula", new CapsulaDoTempoViewModel()
          {
            Mensagem = obj.mensagem,
            ImagemStr = obj.imagem,
            Chave = obj.chave,
            DataAbertura = obj.dataAbertura,
            Duracao = obj.duracao,
            Email = obj.email
          });
        }
        else
        {
          return View("CapsulaExistente", new CapsulaDoTempoViewModel()
          {
            Mensagem = obj.mensagem,
            ImagemStr = obj.imagem
          });
        }
      }
      else if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
      {
        return View("Error");
      }
      return View();

    }

    [ValidateRecaptcha]
    [HttpPost("{id}")]
    public async Task<IActionResult> CriarCapsula(string id, [FromForm] CapsulaDoTempoViewModel capsula)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.Mensagem = "Favor preencher o captcha.";
        return await Index(id);
      }

      string strImagem = string.Empty;
      if (capsula.Imagem != null)
      {
        var str = capsula.Imagem.OpenReadStream();

        byte[] img = new byte[capsula.Imagem.Length];

        using (var memoryStream = new MemoryStream())
        {
          await capsula.Imagem.CopyToAsync(memoryStream);
          img = memoryStream.ToArray();
        }
        strImagem = Convert.ToBase64String(img);
      }

      TimeZoneInfo tz;
      try
      {
        tz = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
      }
      catch (TimeZoneNotFoundException)
      {
        tz = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
      }

      DateTime dataUTC = TimeZoneInfo.ConvertTimeToUtc(capsula.DataAbertura, tz);

      var ipCliente = this.Request.HttpContext.Connection.RemoteIpAddress;


      CapsulaDto caps = new CapsulaDto()
      {
        DataAbertura = dataUTC,
        Mensagem = capsula.Mensagem,
        Imagem = strImagem,
        Duracao = capsula.Duracao,
        Email = capsula.Email,
        Ip = ipCliente.ToString()
      };

      HttpClient cli = new HttpClient();
      var result = await cli.PostAsync(new Uri(urlApi + id), new StringContent(JsonConvert.SerializeObject(caps), Encoding.UTF8, "application/json"));

      if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
      {
        ViewBag.Mensagem = await result.Content.ReadAsStringAsync();
      }

      return await Index(id);
    }

    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public class CapsulaDto
    {
      public DateTime DataAbertura { get; set; }
      public string Mensagem { get; set; }
      public string Imagem { get; set; }
      public DuracaoCapsula Duracao { get; internal set; }
      public string Email { get; internal set; }
      public string Ip { get; internal set; }
    }
  }
}
