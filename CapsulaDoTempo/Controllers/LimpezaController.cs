using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DomainModel.Interfaces.Repositories;
using DomainService;
using Microsoft.AspNetCore.Mvc;

namespace CapsulaDoTempo.Controllers
{
  [Route("api/[controller]")]
  public class LimpezaController : Controller
  {
    IRepositorioCapsulaDoTempo repositorio;

    public LimpezaController(IRepositorioCapsulaDoTempo _repositorio)
    {
      repositorio = _repositorio;
    }

    [HttpPost]
    public async Task Post()
    {
      var ls = new LimpezaService(repositorio);
      await ls.LimparCapsulasExpiradas();
    }
  }
}
