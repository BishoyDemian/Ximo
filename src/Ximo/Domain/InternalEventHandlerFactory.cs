using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ximo.Domain
{
    /// <summary>
    ///     Factory providing logic for creating the <see cref="InternalEventHandler{TDomainEvent}" /> to handle multiple
    ///     domain event handler registrations for the same event.
    /// </summary>
    internal static class InternalEventHandlerFactory
    {
        private static readonly ConcurrentDictionary<Type, List<Type>> EventHandlerMapping =
            new ConcurrentDictionary<Type, List<Type>>();

        /// <summary>
        ///     Builds the internal event handler.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
        /// <typeparam name="TDomainEventHandler">The type of the domain event handler.</typeparam>
        /// <param name="provider">The <see cref="IServiceProvider" /> instance used for registration.</param>
        /// <returns>A new instance of the <see cref="InternalEventHandler{TDomainEvent}" />.</returns>
        public static InternalEventHandler<TDomainEvent> BuildInternalEventHandler<TDomainEvent,
            TDomainEventHandler>(IServiceProvider provider) where TDomainEvent : class
            where TDomainEventHandler : IDomainEventHandler<TDomainEvent>
        {
            //1. Get the event type
            var eventType = typeof(TDomainEvent);

            //2. Check if the event type is contained in the mapping cache. If it is not then initialize it accordingly.
            if (!EventHandlerMapping.ContainsKey(eventType))
            {
                EventHandlerMapping[eventType] = new List<Type>();
            }

            //3. Get the domain event handler type.
            var domainEventHandlerType = typeof(TDomainEventHandler);

            //4. Add the new domain event handler type to the cache if it does not exist.
            if (!EventHandlerMapping[eventType].Contains(domainEventHandlerType))
            {
                EventHandlerMapping[eventType].Add(domainEventHandlerType);
            }

            //5. Construct and return the new instance of the internal event handler.
            var internalEventHandler = new InternalEventHandler<TDomainEvent>(provider);

            //6. Add all the domain event handler types from the mapping cache.
            foreach (var handlerType in EventHandlerMapping[eventType])
            {
                internalEventHandler.Add(handlerType);
            }
            return internalEventHandler;
        }
    }
}