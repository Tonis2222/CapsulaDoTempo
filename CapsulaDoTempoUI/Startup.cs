using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace CapsulaDoTempoUI
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
      services.AddTransient<IConfiguration>(a => Configuration);
      services.AddMvc();
      services.AddRecaptcha(new RecaptchaOptions
      {
        SiteKey = "6LeWc5QUAAAAADqPDP3-ZaIH_bxzpQNxSJhZht4a",
        SecretKey = "6LeWc5QUAAAAAHMrhYcJKfSJkk3jfGnc6_eJJBsN"
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseBrowserLink();
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }
      app.UseRequestLocalization(new RequestLocalizationOptions() { DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR") });
      app.UseStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");

      });
    }
  }
}
