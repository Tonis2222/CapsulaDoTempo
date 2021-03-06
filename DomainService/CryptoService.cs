﻿using DomainModel.Entities;
using DomainModel.Interfaces.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DomainService
{
  public class CryptoService : ICryptoService
  {
    private const string CHAVE_PRIVADA = "CAPSULA_DO_TEMPO";

    public string Criptografar(string id, string conteudo)
    {
      return Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: conteudo,
        salt: Encoding.UTF32.GetBytes(CHAVE_PRIVADA + id),
        prf: KeyDerivationPrf.HMACSHA1,
        iterationCount: 1000,
        numBytesRequested: 256 / 8));
    }

  }
}
