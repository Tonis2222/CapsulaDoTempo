﻿using DomainModel.Entities;
using DomainModel.Interfaces.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositorioMongo
{
  public class RepositorioCapsulaDoTempo : IRepositorioCapsulaDoTempo
  {

    private const string NOME_BASE_CAPSULAS = "capsulas";
    IMongoClient clienteMongoDB;
    IMongoDatabase baseCapsulas;
    IMongoCollection<CapsulaDoTempo> capsulas;


    public RepositorioCapsulaDoTempo(string connectionString)
    {
      this.clienteMongoDB = new MongoClient(connectionString);

      baseCapsulas = clienteMongoDB.GetDatabase(NOME_BASE_CAPSULAS);

      capsulas = baseCapsulas.GetCollection<CapsulaDoTempo>(nameof(CapsulaDoTempo));

      if (capsulas == null)
      {
        baseCapsulas.CreateCollection(nameof(CapsulaDoTempo));

        capsulas = baseCapsulas.GetCollection<CapsulaDoTempo>(nameof(CapsulaDoTempo));
      }
    }

    public async Task ExcluirCapsula(string id)
    {
      await capsulas.DeleteOneAsync(Builders<CapsulaDoTempo>.Filter.Eq(a => a.Id, id));
    }

    public async Task<CapsulaDoTempo> RecuperarCapsula(string id)
    {
      var retorno = await capsulas.FindAsync(Builders<CapsulaDoTempo>.Filter.Eq(a => a.Id, id));
      return retorno.FirstOrDefault();
    }

    public async Task CriarCapsula(CapsulaDoTempo capsula)
    {
      await capsulas.InsertOneAsync(capsula);
    }

    public async Task<List<CapsulaDoTempo>> RecuperarCapsulasAbertasOuExpiradas()
    {
      var result = await capsulas.FindAsync(Builders<CapsulaDoTempo>.Filter.Gt(a => a.DataAbertura, DateTime.UtcNow));
      return result.ToList();

    }

    public async Task AtualizarCapsula(CapsulaDoTempo capsula)
    {
      await capsulas.FindOneAndReplaceAsync(Builders<CapsulaDoTempo>.Filter.Eq(a => a.Id, capsula.Id), capsula);
    }
  }
}

