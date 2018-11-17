using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Interfaces.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositorioMongo;

namespace CapsulaDoTempo
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      string cnn = @"mongodb://capsula-do-tempo:70zdmvtoXYgxuMtbNSxRpViVYZ7eGolxFAfSfSHCpIbub0XybWGY0XQZIJYTvvLMK7hwDjxBUgfePMOdiXfPZg==@capsula-do-tempo.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
      //string cnn2 = @"mongodb+srv://tonis222:DewkAtdethudlo9@capsuladotempo-7usjc.gcp.mongodb.net/test?retryWrites=true";
      services.AddTransient<IRepositorioCapsulaDoTempo, RepositorioCapsulaDoTempo>(a => new RepositorioCapsulaDoTempo(cnn));

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();
    }
  }
}
