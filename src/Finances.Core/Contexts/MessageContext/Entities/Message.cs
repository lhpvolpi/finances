using Finances.Core.Contexts.MessageContext.Enums;
using Finances.Core.Contexts.SharedContext.Entities;

namespace Finances.Core.Contexts.MessageContext.Entities
{
    [Table("messages")]
    public class Message : Entity
    {
        public Message() { }

        public Message(EMessageType type, string payload)
        {
            this.Type = type;
            this.Payload = payload;
        }

        [Column("type")]
        public EMessageType Type { get; set; }

        [Column("payload")]
        public string Payload { get; set; }

        [Column("processed")]
        public bool Processed { get; set; } = false;

        [Column("processing_at")]
        public DateTime? ProcessingAt { get; set; } = null;

        public void MarkAsProcessed()
        {
            this.Processed = true;
            this.ProcessingAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}