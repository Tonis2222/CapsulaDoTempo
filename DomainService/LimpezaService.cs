using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
  public class LimpezaService
  {
    IRepositorioCapsulaDoTempo repositorio;

    public LimpezaService(IRepositorioCapsulaDoTempo _repositorio)
    {
      repositorio = _repositorio;
    }

    public async Task LimparCapsulasExpiradas()
    {
      var capsulasExpiradas = await repositorio.RecuperarCapsulasAbertasOuExpiradas();
      foreach (var capsula in capsulasExpiradas)
      {
        if (capsula.Estado == EstadoCapsula.Expirada)
        {
          await repositorio.ExcluirCapsula(capsula.Id);
        }
      }

    }
  }
}
