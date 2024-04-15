using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Announcements.Controllers
{
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepository _announcementrepository;
        public AnnouncementController(IAnnouncementRepository announcementrepo){
            _announcementrepository = announcementrepo;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<Announcement>>> GetAllAnnouncements(){
            return await _announcementrepository.GetAllAnnouncements();
        }

    }
}