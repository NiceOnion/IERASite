using System;
using Google.Cloud.Firestore;
using Microsoft.VisualBasic;

namespace Announcements.Data
{
    [FirestoreData]
    public class Announcement
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? UserID { get; set; }

        [FirestoreProperty]
        public string? Title { get; set; }

        [FirestoreProperty]
        public string? Body { get; set; }

        [FirestoreProperty]
        public string? Image { get; set; }

        [FirestoreProperty]
        public Timestamp PostTime { get; set; }
    }
}