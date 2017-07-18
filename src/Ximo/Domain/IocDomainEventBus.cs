using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ximo.Domain
{
    /// <summary>
    ///     A default implementation of the <see cref="IDomainEventBus" /> using IOC to deliver published events to the
    ///     relevant handlers.
    /// </summary>
    /// <seealso cref="Ximo.Domain.IDomainEventBus" />
    internal class IocDomainEventBus : IDomainEventBus
    {
        private static readonly Type HandlerType = typeof(IDomainEventHandler<>);
        private readonly IServiceProvider _serviceProvider;

        public IocDomainEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///     Publishes the specified domain event. Delivers the event to the registered/subscribed domain event handlers.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
        /// <param name="event">The domain event.</param>
        /// <param name="throwExceptionWhenNotSubscribedTo">Throw an exception when the event has no subscribers</param>
        /// <remarks>
        ///     If the event is a concrete type, the interfaces of the event will be looked up and in turn any subscriber to the
        ///     concrete type or any of the interfaces will be called. Order is not guaranteed in case of multiple exceptions
        ///     except that the concrete type handler will be the last to be called. This does not include support for class
        ///     inheritance and as such base classes will be ignored when searching for subscribers.
        /// </remarks>
        public void Publish<TDomainEvent>(TDomainEvent @event, bool throwExceptionWhenNotSubscribedTo = true)
            where TDomainEvent : class
        {
            var concreteEventType = @event.GetType();
            var interfaces = concreteEventType.GetTypeInfo().GetInterfaces().ToList();

            var typesToProcess = interfaces;
            typesToProcess.Add(concreteEventType);

            var handlerFound = false;

            foreach (var eventType in typesToProcess)
            {
                var typeToBeResolved = HandlerType.MakeGenericType(eventType);
                var handler = _serviceProvider.GetService(typeToBeResolved);

                if (handler != null)
                {
                    handlerFound = true;
                    typeToBeResolved.GetRuntimeMethod("Handle", new[] {eventType})
                        .Invoke(handler, new object[] {@event});
                }
            }

            if (throwExceptionWhenNotSubscribedTo && !handlerFound)
            {
                throw new InvalidOperationException(
                    $"The event of type '{concreteEventType.FullName}' has no registered event handlers.");
            }
        }

        /// <summary>
        ///     Publishes the specified domain event asynchronously. Delivers the domain event to the registered/subscribed domain
        ///     event handlers.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
        /// <param name="event">The event.</param>
        /// <param name="throwWhenNotSubscribedTo">Throw an exception when the event has no subscribers</param>
        /// <returns>Task.</returns>
        public async Task PublishAsync<TDomainEvent>(TDomainEvent @event, bool throwWhenNotSubscribedTo = true)
            where TDomainEvent : class
        {
            await Task.Factory.StartNew(() => { Publish(@event, throwWhenNotSubscribedTo); });
        }
    }
}