﻿using Google.Cloud.Firestore;

namespace Comments.Models
{
    [FirestoreData]
    public class Comment
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? PostId { get; set; }

        [FirestoreProperty]
        public string? UserId { get; set; }

        [FirestoreProperty]
        public string? Body { get; set; }

        [FirestoreProperty]
        public Timestamp PostTime { get; set; }
    }

}
