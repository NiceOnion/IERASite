using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Announcements.Data
{
    public class Announcement
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? UserID { get; set; }

        public string? Title { get; set; }

        public string? Body { get; set; }

        public string? Image { get; set; }

        public DateTime PostTime { get; set; }
        public int? CommentCount { get; set; } = 0;
    }
}