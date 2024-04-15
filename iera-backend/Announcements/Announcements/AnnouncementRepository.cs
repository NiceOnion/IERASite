using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcements
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly List<Announcement> announcements = new List<Announcement>();

        public AnnouncementRepository(){
            announcements.Add(new Announcement{
                Id = Guid.NewGuid(),
                Title = "Kamer gezocht",
                Body = "Ik ben opzoek naar een kamer in Tilburg. Help!",
                Image = "Dit is een plaatje",
                PostTime = "12:04"
            });

                announcements.Add(new Announcement{
                Id = Guid.NewGuid(),
                Title = "Mijn super toffe tech project",
                Body = "Kijk naar mijn super toffe project!",
                Image = "Project foto",
                PostTime = "02:14"
            });
        }

        public Task<List<Announcement>> GetAllAnnouncements()
        {
            return Task.FromResult(announcements);
        }
    }
}