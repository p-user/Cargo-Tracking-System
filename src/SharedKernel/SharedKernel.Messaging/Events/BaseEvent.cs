
using SharedKernel.Core.DDD;

namespace SharedKernel.Messaging
{
    public record BaseEvent : IDomainEvent
    {
        public Guid Id => Guid.NewGuid();
        public DateTime OccurredOn => DateTime.Now;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
