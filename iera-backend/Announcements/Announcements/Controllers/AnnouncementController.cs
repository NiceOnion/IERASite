using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Announcements.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Announcements.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementController(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository ?? throw new ArgumentNullException(nameof(announcementRepository));
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<Announcement>>> GetAllAnnouncements()
        {
            try
            {
                List<Announcement> announcements = await _announcementRepository.GetAllAnnouncements();
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement(string id)
        {
            try
            {
                Announcement announcement = await _announcementRepository.GetAnnouncement(id);
                if (announcement == null)
                {
                    return NotFound();
                }
                return Ok(announcement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Announcement>> AddAnnouncement([FromBody] Announcement announcement)
        {
            if (announcement == null)
            {
                return BadRequest("Announcement is null");
            }

            try
            {
                await _announcementRepository.AddAnnouncement(announcement);
                return CreatedAtAction(nameof(GetAnnouncement), new { id = announcement.Id }, announcement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAnnouncement([FromBody] Announcement announcement)
        {
            if (announcement == null)
            {
                return BadRequest("Announcement is null");
            }

            try
            {
                bool updated = await _announcementRepository.UpdateAnnouncement(announcement);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(string id)
        {
            try
            {
                bool deleted = await _announcementRepository.DeleteAnnouncement(id);
                if (!deleted)
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
    }
}
