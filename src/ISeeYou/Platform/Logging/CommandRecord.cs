using ISeeYou.Platform.Domain.Messages;
using ISeeYou.Platform.Mongo;
using MongoDB.Bson;

namespace ISeeYou.Platform.Logging
{
    public class CommandRecord
    {
        public BsonDocument CommandDocument { get; set; }
        public CommandMetadata Metadata { get; set; }
        public CommandHandlerRecordCollection Handlers { get; set; }

        public static CommandRecord FromBson(BsonDocument doc)
        {
            var commandDocument = doc.GetBsonDocument("Command");

            var record = new CommandRecord
            {
                CommandDocument = commandDocument,
                Metadata = commandDocument.GetBsonDocument("Metadata").CreateCommandMetadata(),
                Handlers = CommandHandlerRecordCollection.FromBson(doc.GetBsonArray("Handlers"))
            };

            return record;
        }        
    }
}