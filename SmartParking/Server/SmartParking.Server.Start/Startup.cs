using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartParking.Server.DAL.EFCore;
using SmartParking.Server.Service;

namespace SmartParking.Server.Start
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
            // services: ��������

            // Build DbContext
            services.AddDbContext<MySqlDbContext>
            (opts =>
                {
                    opts.UseMySql(Configuration.GetConnectionString("MySQL"),
                    MySqlServerVersion.LatestSupportedServerVersion);
                }
            );

            #region ��ȡָ�������ļ�
            //var builder = new ConfigurationBuilder()
            //          .SetBasePath(Directory.GetCurrentDirectory())
            //          .AddJsonFile("appsettings.json");

            //IConfigurationRoot configurationRoot = builder.Build(); 
            #endregion

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMenuService, MenuService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
