using Autofac;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Autofac.Extras.DynamicProxy;
using Application.Interface;
using Entities.Concrete;
using AutoMapper;
using Application.Services;
using DataAccess.Interfaces;
using DataAccess.Repositories;

namespace Application.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //servis eklendiğinde burada bağımlılığını oluşturun !!!! 
            //entity framework dependency
            builder.RegisterType<ProductRepository>().As<IProductDal>().SingleInstance();
            builder.RegisterType<InformationRepository>().As<IInformationDal>().SingleInstance();
            builder.RegisterType<PhysicalInformationRepository>().As<IPhysicalInformationDal>().SingleInstance();
            builder.RegisterType<PropertyRepository>().As<IPropertyDal>().SingleInstance();
            builder.RegisterType<StateRepository>().As<IStateDal>().SingleInstance();
            builder.RegisterType<StockRepository>().As<IStockDal>().SingleInstance();
            builder.RegisterType<StoreRepository>().As<IStoreDal>().SingleInstance();
            builder.RegisterType<TypeRepository>().As<ITypeDal>().SingleInstance();
            builder.RegisterType<VehicleRepository>().As<IVehicleDal>().SingleInstance();
            builder.RegisterType<Mapper>().As<IMapper>().SingleInstance();

            //manager dependency
            builder.RegisterType<ProductManager>().As<IProductServices>().SingleInstance();
            builder.RegisterType<InformationManager>().As<IInformationServies>().SingleInstance();
            builder.RegisterType<StoreManager>().As<IStoreServices>().SingleInstance();
            builder.RegisterType<StockManager>().As<IStockServices>().SingleInstance();
            builder.RegisterType<VehicleManager>().As<IVehicleServices>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
