using DomainModel.Entities;

namespace DomainService
{
  public class ResultadoBuscaCapsula
  {
    public ResultadoBusca ResultadoBusca { get; internal set; }
    public CapsulaDoTempo Capsula { get; internal set; }
  }
}