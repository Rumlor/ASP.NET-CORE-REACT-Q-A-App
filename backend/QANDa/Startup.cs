using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DbUp;
using System.Reflection;
using DbUp.Engine;
using QANDa.Data;
using QANDa.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using QANDa.Auth;
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


            services
                .AddHttpClient()
                .AddSingleton<IDataRepositoryRead, DataRepository>()
                .AddSingleton<IDataRepositoryWrite, DataRepository>()
                .AddSingleton<IService, QuestionAnswerService>()
                .AddSingleton<IDataCache, DataCache>()
                .AddScoped<IAuthorizationHandler, MustBeQuestionAuthorHandler>()
                .AddHttpContextAccessor()
                .AddControllers();

            //add auth service
            services
                .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:Authority"];
                options.Audience = Configuration["Auth0:Audience"];
            });
            services
                .AddAuthorization(options => 
                
                    options.AddPolicy("MustBeQuestionAuthor", policy => 
                    
                        policy.AddRequirements(new MustBeQuestionAuthorRequirement())
                    )                
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(app=> { app.MapControllers(); });
        }
    }
}
