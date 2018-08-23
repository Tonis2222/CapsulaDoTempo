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

namespace CapsulaDoTempoUI.Controllers
{
  public class HomeController : Controller
  {
    string urlApi = "";
    public HomeController(IConfiguration config)
    {
      urlApi = config.GetValue<string>("URLCapsulaDoTempoAPI");



    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
      {
        //Mostra o manual
        return View();
      }

      if (id == "ping")
      {
        return View("CapsulaExistente", new CapsulaDoTempoViewModel() { Mensagem = "exemplo" });
      }

      HttpClient cli = new HttpClient();
      var result = await cli.GetAsync(new Uri(urlApi + id));

      if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
      {
        return View("CapsulaFechada");
      }
      else if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        return View();
      }
      else if (result.StatusCode == System.Net.HttpStatusCode.OK)
      {
        var bdResult = await result.Content.ReadAsStringAsync();
        dynamic obj = Newtonsoft.Json.Linq.JObject.Parse(bdResult);

        return View("CapsulaExistente", new CapsulaDoTempoViewModel()
        {
          Mensagem = obj.mensagem
        });
      }
      return View();

    }

    [HttpPost("{id}")]
    public async Task<IActionResult> CriarCapsula(string id, [FromForm] CapsulaDoTempoViewModel capsula)
    {
      var str = capsula.Imagem.OpenReadStream();

      byte[] img = new byte[capsula.Imagem.Length];

      using (var memoryStream = new MemoryStream())
      {
        await capsula.Imagem.CopyToAsync(memoryStream);
        img = memoryStream.ToArray();
      }

      var strImagem = Convert.ToBase64String(img);

      CapsulaDto caps = new CapsulaDto()
      {
        DataAbertura = capsula.DataAbertura,
        Mensagem = capsula.Mensagem,
        Imagem = strImagem
      };

      HttpClient cli = new HttpClient();
      var result = await cli.PostAsync(new Uri(urlApi + id), new StringContent(JsonConvert.SerializeObject(caps), Encoding.UTF8, "application/json"));

      //foreach (string file in Request..Files)
      //{
      //  HttpPostedFile hpf = Request.Files[file] as HttpPostedFile;
      //  if (hpf.ContentLength == 0)
      //    continue;
      //  string savedFileName = Path.Combine(
      //     AppDomain.CurrentDomain.BaseDirectory,
      //     Path.GetFileName(hpf.FileName));
      //  hpf.SaveAs(savedFileName);
      //}

      return Ok();
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
    }
  }
}
