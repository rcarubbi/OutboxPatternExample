namespace InfrastructureLayer.Database.Outbox
{
    internal class OutboxMessageWrapper
    {
        public string Payload { get; set; } = null!;
        public string MessageType { get; set; } = null!;
    }
}
