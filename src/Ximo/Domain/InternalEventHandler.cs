using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Ximo.Domain
{
    /// <summary>
    ///     The internal event handler provides a mechanism by which to register and call multiple domain event handlers for
    ///     the same event.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
    /// <seealso cref="Ximo.Domain.IDomainEventHandler{TDomainEvent}" />
    internal class InternalEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : class
    {
        private readonly IServiceProvider _provider;

        public InternalEventHandler(IServiceProvider provider)
        {
            _provider = provider;
        }

        private List<Type> HandlerTypes { get; } = new List<Type>();

        /// <summary>
        ///     Handles the specified domain event published through the <see cref="T:Ximo.Domain.IDomainEventBus" /> or another
        ///     mechanism.
        /// </summary>
        /// <param name="event">The published domain event to be handled.</param>
        public void Handle(TDomainEvent @event)
        {
            foreach (var domainEventHandler in HandlerTypes)
            {
                var handler = (IDomainEventHandler<TDomainEvent>) _provider.GetRequiredService(domainEventHandler);
                handler.Handle(@event);
            }
        }

        /// <summary>
        ///     Adds the specified domain event handler type into the list of handlers to be executed when the event is published.
        /// </summary>
        /// <param name="domainEventHandler">The type domain event handler.</param>
        public void Add(Type domainEventHandler)
        {
            HandlerTypes.Add(domainEventHandler);
        }
    }
}