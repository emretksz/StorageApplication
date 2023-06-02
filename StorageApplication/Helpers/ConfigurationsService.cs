using Application.Interface;
using Application.Services;
using IoC;
using Microsoft.Extensions.DependencyModel;

namespace StorageApplication.Helpers
{
    public class ConfigurationsService
    {
        public void RegisterServices(IServiceCollection services)
        {
            // Diğer servis kayıtları
            services.AddTransient<IDependency, Dependency>();
            services.AddTransient<IProductServices,ProductManager>();
            services.AddTransient<IStockServices,StockManager>();
            services.AddTransient<IStoreServices,StoreManager>();
            services.AddTransient<IVehicleServices,VehicleManager>();
            services.AddTransient<IInformationServies,InformationManager>();


        }
    }
}
