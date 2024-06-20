using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Users.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("username")]
        public string? username { get; set; }
        [BsonElement("email")]
        public string? email { get; set; }
        [BsonElement("password")]
        public string? password { get; set; }
        [BsonElement("BirthDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? BirthDate { get; set; }
    }
}
