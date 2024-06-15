using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Announcements.Data
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly IMongoDBContext _context;


        public AnnouncementRepository(IMongoDBContext context)
        {
            _context = context;
        }

        public async Task<List<Announcement>> GetAllAnnouncements()
        {
            return await _context.Announcements.Find(_ => true).ToListAsync();
        }

        public async Task<List<Announcement>> GetAllAnnouncementsByUser(string userId)
        {
            var filter = Builders<Announcement>.Filter.Eq(a => a.UserID, userId);
            return await _context.Announcements.Find(filter).ToListAsync();
        }

        public async Task<Announcement> GetAnnouncement(string id)
        {
            var filter = Builders<Announcement>.Filter.Eq(a => a.Id, id);
            return await _context.Announcements.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAnnouncement(Announcement announcement)
        {
            announcement.Id = ObjectId.GenerateNewId().ToString();
            await _context.Announcements.InsertOneAsync(announcement);
        }

        public async Task<bool> UpdateAnnouncement(Announcement announcement)
        {
            var filter = Builders<Announcement>.Filter.Eq(a => a.Id, announcement.Id);
            var result = await _context.Announcements.ReplaceOneAsync(filter, announcement);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAnnouncement(string id)
        {
            var filter = Builders<Announcement>.Filter.Eq(a => a.Id, id);
            var result = await _context.Announcements.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async void UpdateAnnouncementDeletedAccount(string userId)
        {
            var filter = Builders<Announcement>.Filter.Eq(a => a.UserID, userId);
            var update = Builders<Announcement>.Update.Set(a => a.UserID, "User removed");

            var result = await _context.Announcements.UpdateManyAsync(filter, update);
        }
    }
}
