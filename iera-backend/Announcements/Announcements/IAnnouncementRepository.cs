using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements
{
    public interface IAnnouncementRepository
    {
        public Task<List<Announcement>> GetAllAnnouncements();
    }
}