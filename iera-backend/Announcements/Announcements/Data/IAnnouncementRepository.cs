namespace Announcements.Data
{
    public interface IAnnouncementRepository
    {
        public Task<List<Announcement>> GetAllAnnouncements();
        public Task<List<Announcement>> GetAllAnnouncementsByUser(string userId);
        public Task<Announcement> GetAnnouncement(string id);
        public Task AddAnnouncement(Announcement announcement);
        public Task<bool> UpdateAnnouncement(Announcement announcement);
        public void UpdateAnnouncementDeletedAccount(string id);
        public Task<bool> DeleteAnnouncement(string id);

    }
}