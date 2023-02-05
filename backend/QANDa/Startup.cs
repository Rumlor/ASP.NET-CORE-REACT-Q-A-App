using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbUp;
using System.Reflection;
using DbUp.Engine;
using QANDa.Data;
using QANDa.Service;

namespace QANDa
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
            string connectionString =  Configuration.GetConnectionString("DefaultConnection");
            EnsureDatabase.For.SqlDatabase(connectionString);
            UpgradeEngine upgrader = DeployChanges.To.SqlDatabase(connectionString)
                                                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                                                .WithTransaction()
                                                .LogToConsole()
                                                .Build();
            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }
            services.AddControllers();
            services.AddSingleton<IDataRepositoryRead, DataRepository>();
            services.AddSingleton<IDataRepositoryWrite,DataRepository>();
            services.AddSingleton<IService,QuestionAnswerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(app=> { app.MapControllers(); });
        }
    }
}
