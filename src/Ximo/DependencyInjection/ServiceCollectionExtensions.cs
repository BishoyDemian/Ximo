using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ximo.Cqrs;
using Ximo.Domain;

namespace Ximo.DependencyInjection
{
    /// <summary>
    ///     Class providing extensions to the <see cref="IServiceCollection" /> to allow for the registration of different
    ///     types of handlers for different message types.
    /// </summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Registers the command handler for the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TCommandHandler">The type of the command handler.</typeparam>
        /// <param name="serviceCollection">The service collection to contain the registration.</param>
        /// <param name="serviceLifetime">The service lifetime. Default is Transient.</param>
        /// <returns>A reference to this service collection isntance after the operation has completed.</returns>
        public static IServiceCollection RegisterCommandHandler<TCommand, TCommandHandler>(
            this IServiceCollection serviceCollection, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TCommand : class
            where TCommandHandler : class, ICommandHandler<TCommand>
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    return serviceCollection.AddSingleton<ICommandHandler<TCommand>, TCommandHandler>();
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped<ICommandHandler<TCommand>, TCommandHandler>();
                default:
                    return serviceCollection.AddTransient<ICommandHandler<TCommand>, TCommandHandler>();
            }
        }

        /// <summary>
        ///     Registers the default command bus <see cref="IocCommandBus" />.
        /// </summary>
        /// <param name="serviceCollection">The service collection to contain the registration.</param>
        /// <param name="serviceLifetime">The service lifetime. Default is Singleton.</param>
        /// <returns>A reference to this service collection isntance after the operation has completed.</returns>
        public static IServiceCollection RegisterDefaultCommandBus(this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    return serviceCollection.AddTransient<ICommandBus, IocCommandBus>();
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped<ICommandBus, IocCommandBus>();
                default:
                    return serviceCollection.AddSingleton<ICommandBus, IocCommandBus>();
            }
        }

        /// <summary>
        ///     Registers the query handler for the specified query and response.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TQueryHandler">The type of the query handler.</typeparam>
        /// <param name="serviceCollection">The service collection to contain the registration.</param>
        /// <param name="serviceLifetime">The service lifetime. Default is Transient.</param>
        /// <returns>A reference to this service collection isntance after the operation has completed.</returns>
        public static IServiceCollection RegisterQueryHandler<TQuery, TResult, TQueryHandler>(
            this IServiceCollection serviceCollection, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TQuery : class
            where TQueryHandler : class, IQueryHandler<TQuery, TResult>
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    return serviceCollection.AddSingleton<IQueryHandler<TQuery, TResult>, TQueryHandler>();
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped<IQueryHandler<TQuery, TResult>, TQueryHandler>();
                default:
                    return serviceCollection.AddTransient<IQueryHandler<TQuery, TResult>, TQueryHandler>();
            }
        }

        /// <summary>
        ///     Registers the default query processor <see cref="IocQueryProcessor" />.
        /// </summary>
        /// <param name="serviceCollection">The service collection to contain the registration.</param>
        /// <param name="serviceLifetime">The service lifetime. Default is Singleton.</param>
        /// <returns>A reference to this service collection isntance after the operation has completed.</returns>
        public static IServiceCollection RegisterDefaultQueryProcessor(this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    return serviceCollection.AddTransient<IQueryProcessor, IocQueryProcessor>();
                case ServiceLifetime.Scoped:
                    return serviceCollection.AddScoped<IQueryProcessor, IocQueryProcessor>();
                default:
                    return serviceCollection.AddSingleton<IQueryProcessor, IocQueryProcessor>();
            }
        }

        /// <summary>
        ///     Registers the domain event handler. The toolset will automatically create an internal handler to aggregate single
        ///     or multiple handlers for the domain event.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
        /// <typeparam name="TDomainEventHandler">The type of the domain event handler.</typeparam>
        /// <param name="serviceCollection">The service collection to contain the registration.</param>
        /// <param name="serviceLifetime">The service lifetime. Default is Transient.</param>
        /// <returns>A reference to this service collection isntance after the operation has completed.</returns>
        /// <remarks>
        ///     .NET Core does not support multiple registrations for the same type. So we have to use a factory and provide an
        ///     internal handler implementation capable of handling multiple event handler registrations for the same event.
        /// </remarks>
        public static IServiceCollection RegisterDomainEventHandler<TDomainEvent, TDomainEventHandler>(
            this IServiceCollection serviceCollection, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
            where TDomainEvent : class
            where TDomainEventHandler : IDomainEventHandler<TDomainEvent>
        {
            var registration =
                serviceCollection.FirstOrDefault(s => s.ServiceType == typeof(IDomainEventHandler<TDomainEvent>));

            if (registration != null)
            {
                serviceCollection.Remove(registration);
            }

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    serviceCollection.AddSingleton(typeof(TDomainEventHandler));
                    break;
                case ServiceLifetime.Scoped:
                    serviceCollection.AddScoped(typeof(TDomainEventHandler));
                    break;
                default:
                    serviceCollection.AddTransient(typeof(TDomainEventHandler));
                    break;
            }

            return serviceCollection.AddSingleton<IDomainEventHandler<TDomainEvent>>(InternalEventHandlerFactory
                .BuildInternalEventHandler<TDomainEvent, TDomainEventHandler>);
        }

        /// <summary>
        ///     Registers the default domain event bus. <see cref="IocDomainEventBus" /> as a singleton.
        /// </summary>
        /// <param name="serviceCollection">The service collection to contain the registration.</param>
        /// <param name="serviceLifetime">The service lifetime. Default is Singleton.</param>
        /// <returns>A reference to this service collection isntance after the operation has completed.</returns>
        public static IServiceCollection RegisterDefaultDomainEventBus(this IServiceCollection serviceCollection,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            return serviceCollection.AddSingleton<IDomainEventBus, IocDomainEventBus>();
        }

        /// <summary>
        ///     Loads a module by applying all the specified registrations and configurations to the supplied service collection.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="serviceCollection">The service collection to contain the registrations.</param>
        /// <param name="configuration">The optional configuration instance to be used for registrations.</param>
        /// <returns>A reference to this service collection isntance after the operation has completed.</returns>
        public static IServiceCollection LoadModule<TModule>(this IServiceCollection serviceCollection,
            IConfiguration configuration = null)
            where TModule : IModule, new()
        {
            var module = new TModule();
            if (configuration != null)
            {
                module.Configuration = configuration;
            }
            module.Initialize(serviceCollection);
            return serviceCollection;
        }
    }
}