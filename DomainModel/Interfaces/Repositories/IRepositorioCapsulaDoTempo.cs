using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces.Repositories
{
  public interface IRepositorioCapsulaDoTempo
  {
    Task<CapsulaDoTempo> RecuperarCapsula(string id);
    Task CriarCapsula(CapsulaDoTempo capsula);
    Task ExcluirCapsula(string id);
    Task<List<CapsulaDoTempo>> RecuperarCapsulasAbertasOuExpiradas();
    Task AtualizarCapsula(CapsulaDoTempo capsula);
  }
}
