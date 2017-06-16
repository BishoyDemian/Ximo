using System;
using System.Diagnostics.CodeAnalysis;
using Ximo.Utilities;

namespace Ximo.EventSourcing
{
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public class DomainEventEnvelope
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainEventEnvelope" /> class.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="sequence">The sequence.</param>
        /// <param name="aggregateVersion">The aggregate version.</param>
        /// <param name="domainEvent">The domain event.</param>
        public DomainEventEnvelope(Guid eventId, Guid aggregateId, int sequence, int aggregateVersion,
            object domainEvent)
            : this(aggregateId, domainEvent)
        {
            EventId = eventId;
            Sequence = sequence;
            AggregateVersion = aggregateVersion;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainEventEnvelope" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="sequence">The sequence.</param>
        /// <param name="aggregateVersion">The aggregate version.</param>
        /// <param name="domainEvent">The domain event.</param>
        public DomainEventEnvelope(Guid aggregateId, int sequence, int aggregateVersion, object domainEvent)
            : this(aggregateId, domainEvent)
        {
            EventId = GuidFactory.NewGuidComb();
            Sequence = sequence;
            AggregateVersion = aggregateVersion;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainEventEnvelope" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="domainEvent">The domain event.</param>
        public DomainEventEnvelope(Guid aggregateId, object domainEvent)
        {
            AggregateId = aggregateId;
            CreatedOnUtc = DateTime.UtcNow;
            Event = domainEvent;
            EventName = domainEvent.GetType().FullName;
        }

        /// <summary>
        ///     Gets the event.
        /// </summary>
        public object Event { get; private set; }

        /// <summary>
        ///     Gets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        public string EventName { get; private set; }

        /// <summary>
        ///     Gets the event identifier.
        /// </summary>
        /// <value>The event identifier.</value>
        public Guid EventId { get; private set; }

        /// <summary>
        ///     Gets the unique identifier of the aggregate.
        /// </summary>
        /// <value>The aggregate identifier.</value>
        public Guid AggregateId { get; private set; }

        /// <summary>
        ///     Gets the event sequence number.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence { get; private set; }

        /// <summary>
        ///     Gets the date and time when the event was created (UTC).
        /// </summary>
        public DateTime CreatedOnUtc { get; private set; }

        /// <summary>
        ///     Gets the version of the aggregate.
        /// </summary>
        /// <value>The aggregate version.</value>
        public int AggregateVersion { get; private set; }
    }
}