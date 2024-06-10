using Comments.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Comments.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<ActionResult> AddComment([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment cannot be null.");
            }

            DocumentReference newComment = await _commentRepository.Add(comment);
            return CreatedAtAction(nameof(GetCommentById), new { id = newComment.Id }, comment);
        }

        // DELETE: api/Comment/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(string id)
        {
            bool result = await _commentRepository.DeleteAnnouncement(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // GET: api/Comment/Post/{postId}
        [HttpGet("Post/{postId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsByPost(string postId)
        {
            List<Comment> comments = await _commentRepository.GetAllCommentsFromPost(postId);
            if (comments == null || comments.Count == 0)
            {
                return NotFound();
            }
            return Ok(comments);
        }

        // GET: api/Comment/User/{userId}
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsByUser(string userId)
        {
            List<Comment> comments = await _commentRepository.GetAllCommentsFromUser(userId);
            if (comments == null || comments.Count == 0)
            {
                return NotFound();
            }
            return Ok(comments);
        }

        // PUT: api/Comment/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComment(string id, [FromBody] Comment comment)
        {
            if (comment == null || comment.Id != id)
            {
                return BadRequest("Comment ID mismatch.");
            }

            bool result = await _commentRepository.UpdateAnnouncement(comment);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }

        // GET: api/Comment/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(string id)
        {
            var comments = await _commentRepository.GetAllCommentsFromPost(id);
            if (comments == null)
            {
                return NotFound();
            }
            return Ok(comments);
        }
    }
}
