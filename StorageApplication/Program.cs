using Application.DependencyResolvers.Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using WebApp;
using Microsoft.Extensions.Hosting;
using DataAccess.EntityFramework;

namespace StorageApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
           .ConfigureContainer<ContainerBuilder>(builder =>
           {
               builder.RegisterModule(new AutofacBusinessModule());
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
           })
           .ConfigureServices((hostContext, services) =>
           {
               services.AddHostedService<RefresDatabaseConfiguration>();
           });
    }
}
