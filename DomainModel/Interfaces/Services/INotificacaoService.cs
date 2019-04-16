using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces.Services
{
  public interface INotificacaoService
  {
    Task NotificarCriacaoCapsula(CapsulaDoTempo capsula);
  }
}
