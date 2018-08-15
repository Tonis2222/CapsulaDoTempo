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

      //if (configuracoes == null)
      //{
      //  baseConfiguracao.CreateCollection(nameof(ConfiguracaoDeConversao));

      //  configuracoes = baseConfiguracao.GetCollection<ConfiguracaoDeConversao>(nameof(ConfiguracaoDeConversao));
      //}
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

