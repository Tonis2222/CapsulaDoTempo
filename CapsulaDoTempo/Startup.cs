using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Interfaces;
using DomainModel.Interfaces.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificacaoEmail;
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
      string cnn = @"mongodb://capsuladotempomongo:pa0gMV43v35N2R7fWfQTy2fhVT8SrTuqYXFJo5EHbMeFsaoySfH8BiYyY1JjmKqFlG1BgRyCcattKavcAWTlMQ==@capsuladotempomongo.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
      string senhaGmail = "2794xxXX";

      services.AddTransient<IRepositorioCapsulaDoTempo, RepositorioCapsulaDoTempo>(a => new RepositorioCapsulaDoTempo(cnn));
      services.AddTransient<INotificacaoService, NotificadorGmail>(a => new NotificadorGmail(senhaGmail));
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
