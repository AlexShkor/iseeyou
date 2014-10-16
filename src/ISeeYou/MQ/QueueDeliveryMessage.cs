namespace ISeeYou.MQ
{
    public class QueueDeliveryMessage<TRabbitEvent> where TRabbitEvent : RabbitEventBase
    {
        public ulong DeliveryTag { get; set; }
        public TRabbitEvent Event { get; set; }
    }
}
