using SharedKernel.DDD;
namespace SharedKernel.Events
{
    public record BaseEvent : IDomainEvent
    {
        public Guid Id => Guid.NewGuid();
        public DateTime OccurredOn => DateTime.Now;
        public string EventType => GetType().AssemblyQualifiedName;
    }
}
