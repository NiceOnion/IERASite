using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements.Data
{
    public interface IAnnouncementRepository
    {
        public Task<List<Announcement>> GetAllAnnouncements();
        public Task<Announcement> GetAnnouncement(string id);
        public Task<DocumentReference> AddAnnouncement(Announcement announcement);
        public Task<bool> UpdateAnnouncement(Announcement announcement);
        public Task<bool> DeleteAnnouncement(string id);

    }
}