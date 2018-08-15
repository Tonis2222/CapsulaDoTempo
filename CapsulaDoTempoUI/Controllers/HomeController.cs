using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CapsulaDoTempoUI.Models;

namespace CapsulaDoTempoUI.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("{id}")]
    public IActionResult Index(string id)
    {
      return View();
    }

    public IActionResult CriarCapsula(string id)
    {
      return Ok();
    }

    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
