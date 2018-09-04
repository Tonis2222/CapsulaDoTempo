using Modelo;
using MongoDB.Driver;
using System;
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

    public async Task SalvarCapsula(CapsulaDoTempo capsula)
    {
     await capsulas.InsertOneAsync(capsula);
    }
  }
}

