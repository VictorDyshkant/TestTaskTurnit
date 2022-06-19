using Autofac;
using AutoMapper;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using NHibernate;
using Turnit.Abstraction.Repositories;
using Turnit.Abstraction.Services;
using Turnit.Abstraction.UnitOfWork;
using Turnit.Database.Repositories;
using Turnit.Database.UnitOfWork;
using Turnit.GenericStore.Api.Mapper;
using Turnit.Service.Mapper;
using Turnit.Service.Services;

namespace Turnit.GenericStore.Api.Autofac;

public class AutofacModule : Module
{
    private readonly IConfiguration _configuration;
    public AutofacModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        ConfigureRepositories(builder);
        ConfigureServices(builder);
        ConfigureConfigurationComponents(builder);
    }

    private void ConfigureRepositories(ContainerBuilder builder)
    {
        builder.RegisterInstance(CreateSessionFactory())
            .As<ISessionFactory>()
            .SingleInstance();

        builder.RegisterType<CategoryRepository>()
            .As<ICategoryRepository>()
            .InstancePerDependency();

        builder.RegisterType<StoreRepository>()
            .As<IStoreRepository>()
            .InstancePerDependency();

        builder.RegisterType<ProductRepository>()
            .As<IProductRepository>()
            .InstancePerDependency();

        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerDependency();
    }

    private void ConfigureServices(ContainerBuilder builder)
    {
        builder.RegisterType<CategoryService>()
            .As<ICategoryService>()
            .InstancePerDependency();

        builder.RegisterType<ProductService>()
            .As<IProductService>()
            .InstancePerDependency();

        builder.RegisterType<StoreService>()
            .As<IStoreService>()
            .InstancePerDependency();
    }

    private void ConfigureConfigurationComponents(ContainerBuilder builder)
    {
        builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoToModelMapperProfile>();
                cfg.AddProfile<EntityToDtoMapperProfile>();
            }
        )).AsSelf().SingleInstance();

        builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
    }

    private ISessionFactory CreateSessionFactory()
    {
        var connectionString = _configuration.GetConnectionString("Default");
        var configuration = Fluently.Configure()
            .Database(PostgreSQLConfiguration.PostgreSQL82
                .Dialect<NHibernate.Dialect.PostgreSQL82Dialect>()
                .ConnectionString(connectionString))
            .Mappings(x =>
            {
                x.FluentMappings.AddFromAssemblyOf<UnitOfWork>();
            });

        return configuration.BuildSessionFactory();
    }
}