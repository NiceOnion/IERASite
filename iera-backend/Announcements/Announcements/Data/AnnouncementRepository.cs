using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading.Tasks;

namespace Announcements.Data
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private FirestoreDb _db;

        public AnnouncementRepository()
        {
            _db = FirestoreDb.Create("ierasite-d02be");
        }

        public async Task<List<Announcement>> GetAllAnnouncements()
        {
            CollectionReference usersCollection = _db.Collection("Announcements");
            QuerySnapshot snapshot = await usersCollection.GetSnapshotAsync();

            List<Announcement> users = new List<Announcement>();

            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Announcement announcement = document.ConvertTo<Announcement>();
                    users.Add(announcement);
                }
            }

            return users;
        }

        public async Task<Announcement> GetAnnouncement(string id)
        {
            DocumentReference docRef = _db.Collection("Announcements").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                Announcement user = snapshot.ConvertTo<Announcement>();
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<DocumentReference> AddAnnouncement(Announcement announcement)
        {
            CollectionReference announcementRef = _db.Collection("Announcements");
            DocumentReference newUserRef = await announcementRef.AddAsync(announcement);
            return newUserRef;
        }

        public async Task<bool> UpdateAnnouncement(Announcement announcement)
        {
            DocumentReference docRef = _db.Collection("Announcements").Document(announcement.Id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                await docRef.SetAsync(announcement, SetOptions.MergeAll);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAnnouncement(string id)
        {
            DocumentReference docRef = _db.Collection("Announcements").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                await docRef.DeleteAsync();
                return true;
            }
            return false;
        }
    }
}
