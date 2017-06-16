using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ximo.Domain
{
    /// <summary>
    ///     A default implementation of the <see cref="IDomainEventBus" /> using IOC to deliver published events to the
    ///     relevant handlers.
    /// </summary>
    /// <seealso cref="Ximo.Domain.IDomainEventBus" />
    internal class IocDomainEventBus : IDomainEventBus
    {
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
        /// <param name="throwWhenNotSubscribedTo">Throw an exception when the event has no subscribers</param>
        public void Publish<TDomainEvent>(TDomainEvent @event, bool throwWhenNotSubscribedTo = true)
            where TDomainEvent : class
        {
            var handlerType = typeof(IDomainEventHandler<>);
            var eventType = @event.GetType();
            var typeToBeResolved = handlerType.MakeGenericType(eventType);

            var handler = throwWhenNotSubscribedTo
                ? _serviceProvider.GetRequiredService(typeToBeResolved)
                : _serviceProvider.GetService(typeToBeResolved);
            typeToBeResolved.GetRuntimeMethod("Handle", new Type[] {eventType}).Invoke(handler, new object[]{@event});
            //handler.Handle(@event);
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