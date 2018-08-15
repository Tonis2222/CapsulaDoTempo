using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
  public interface IRepositorioCapsulaDoTempo
  {
    Task<CapsulaDoTempo> RecuperarCapsula(string id);
    Task SalvarCapsula(CapsulaDoTempo capsula);
    Task ExcluirCapsula(string id);
  }
}
