using ISeeYou.Platform.Domain.Messages;

namespace ISeeYou.Platform.Domain.Interfaces
{
    /// <summary>
    /// Domain Event
    /// </summary>
    public interface IEvent
    {
        string Id { get; set; }
        EventMetadata Metadata { get; set; }
    }
}