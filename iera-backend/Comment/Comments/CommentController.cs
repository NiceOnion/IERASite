using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comments.Data; // Ensure this namespace includes your CommentRepository and other necessary classes
using Comments.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Comments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public CommentController(ICommentRepository commentRepository, IServiceProvider serviceProvider)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _serviceProvider = serviceProvider;
            _rabbitMQPublisher = new RabbitMQPublisher();
        }

        [HttpPost]
        public async Task<ActionResult> AddComment([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment cannot be null.");
            }

            try
            {
                await _commentRepository.Add(comment);
                _rabbitMQPublisher.SendCommentAddedMessage(comment.PostId);
                _rabbitMQPublisher.Close();
                return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(string id)
        {
            try
            {
                bool result = await _commentRepository.DeleteComment(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("Post/{postId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsByPost(string postId)
        {
            try
            {
                List<Comment> comments = await _commentRepository.GetAllCommentsFromPost(postId);
                if (comments == null || comments.Count == 0)
                {
                    return NotFound();
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("User/{userId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsByUser(string userId)
        {
            try
            {
                List<Comment> comments = await _commentRepository.GetAllCommentsFromUser(userId);
                if (comments == null || comments.Count == 0)
                {
                    return NotFound();
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComment(string id, [FromBody] Comment comment)
        {
            if (comment == null || comment.Id != id)
            {
                return BadRequest("Comment ID mismatch.");
            }

            try
            {
                bool result = await _commentRepository.UpdateComment(comment);
                if (result)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(string id)
        {
            try
            {
                Comment comment = await _commentRepository.GetCommentById(id);
                if (comment == null)
                {
                    return NotFound();
                }
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private async void ProcessUserDeleteEvent(string userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {

                // Get all announcements related to the user
                var announcements = await _commentRepository.GetAllCommentsFromUser(userId);

                // Update announcements to mark user deleted
                foreach (var announcement in announcements)
                {
                    announcement.UserId = "UserDeleted"; // Or set to a specific value indicating user deletion
                    await _commentRepository.UpdateComment(announcement);
                }

                Console.WriteLine($"Processed user delete event for announcements. User ID: {userId}");
            }
        }
    }
}
