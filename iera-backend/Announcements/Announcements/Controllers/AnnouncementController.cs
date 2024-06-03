using Announcements.Data;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Announcements.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepository _announcementrepository;
        public AnnouncementController(IAnnouncementRepository announcementrepo){
            _announcementrepository = announcementrepo;
        }


        [HttpGet]
        [Route("All")]
        public async Task<ActionResult<List<Announcement>>> GetAllAnnouncements()
        {    
            List<Announcement> Announcements =  await _announcementrepository.GetAllAnnouncements();
            return Ok(Announcements);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement(string id)
        {
            Announcement announcement = await _announcementrepository.GetAnnouncement(id);
            if(announcement == null)
            {
                return NotFound();
            }
            return Ok(announcement);
        }

        [HttpPost]
        public async Task<ActionResult<Announcement>> AddAnnouncement([FromBody] Announcement announcement)
        {
            if(announcement == null)
            {
                return BadRequest("User is null");
            }

            try
            {
                DocumentReference docRef = await _announcementrepository.AddAnnouncement(announcement);
                return CreatedAtAction(nameof(GetAnnouncement), new { id = docRef.Id }, announcement);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAnnouncement ([FromBody] Announcement announcement)
        {
            if (announcement == null)
            {
                return BadRequest("User is null");
            }

            try
            {
                bool updated = await _announcementrepository.UpdateAnnouncement(announcement);
                if (!updated)
                {
                    return NotFound();
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAnnouncement (string id)
        {
            try
            {
                bool deleted = await _announcementrepository.DeleteAnnouncement(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}