using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Interfaces.Services
{
  public interface ICryptoService
  {
    string Criptografar(string id, string conteudo);
  }
}
