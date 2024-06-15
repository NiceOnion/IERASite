using Google.Cloud.Firestore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Comments.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? PostId { get; set; }
        
        public string? UserId { get; set; }

        public string? Body { get; set; }

        public DateTime PostTime { get; set; }
    }

}
