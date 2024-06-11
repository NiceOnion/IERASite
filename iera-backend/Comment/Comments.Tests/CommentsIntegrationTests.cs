using Comments.Controllers;
using Comments.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Comments.Tests
{
    public class CommentIntegrationTests
    {
        private readonly ICommentRepository _repository;
        private readonly CommentController _controller;

        public CommentIntegrationTests()
        {
            _repository = new CommentRepository();
            _controller = new CommentController(_repository);
        }

        [Fact]
        public async Task AddComment_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var comment = new Comment { Body = "This is a test comment", PostId = "post1", UserId = "user1" };

            // Act
            var result = await _controller.AddComment(comment);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Comment>(createdAtActionResult.Value);
            Assert.Equal(comment.Body, returnValue.Body);

            // Clean up
            await _repository.DeleteComment(createdAtActionResult.RouteValues["id"].ToString());
        }

        [Fact]
        public async Task DeleteComment_ReturnsNoContentResult()
        {
            // Arrange
            var comment = new Comment { Body = "This is a test comment", PostId = "post1", UserId = "user1" };
            var docRef = await _repository.Add(comment);

            // Act
            var result = await _controller.DeleteComment(docRef.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteComment_ReturnsNotFoundResult()
        {
            // Act
            var result = await _controller.DeleteComment("non-existent-id");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetCommentsByPost_ReturnsOkResult_WithListOfComments()
        {
            // Arrange
            var comment1 = new Comment { Body = "This is a test comment 1", PostId = "post1", UserId = "user1" };
            var comment2 = new Comment { Body = "This is a test comment 2", PostId = "post1", UserId = "user1" };
            var docRef1 = await _repository.Add(comment1);
            var docRef2 = await _repository.Add(comment2);

            // Act
            var result = await _controller.GetCommentsByPost("post1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Comment>>(okResult.Value);

            // Clean up
            await _repository.DeleteComment(docRef1.Id);
            await _repository.DeleteComment(docRef2.Id);
        }

        [Fact]
        public async Task GetCommentsByPost_ReturnsNotFoundResult_WhenNoComments()
        {
            // Act
            var result = await _controller.GetCommentsByPost("non-existent-post");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCommentsByUser_ReturnsOkResult_WithListOfComments()
        {
            // Arrange
            var comment1 = new Comment { Body = "This is a test comment 1", PostId = "post1", UserId = "user1" };
            var comment2 = new Comment { Body = "This is a test comment 2", PostId = "post2", UserId = "user1" };
            var docRef1 = await _repository.Add(comment1);
            var docRef2 = await _repository.Add(comment2);

            // Act
            var result = await _controller.GetCommentsByUser("user1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Comment>>(okResult.Value);

            // Clean up
            await _repository.DeleteComment(docRef1.Id);
            await _repository.DeleteComment(docRef2.Id);
        }

        [Fact]
        public async Task GetCommentsByUser_ReturnsNotFoundResult_WhenNoComments()
        {
            // Act
            var result = await _controller.GetCommentsByUser("non-existent-user");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateComment_ReturnsNoContentResult()
        {
            // Arrange
            var comment = new Comment { Body = "This is a test comment", PostId = "post1", UserId = "user1" };
            var docRef = await _repository.Add(comment);

            comment.Id = docRef.Id;  // Set the ID to the Firestore-generated ID
            comment.Body = "Updated text";

            // Act
            var result = await _controller.UpdateComment(docRef.Id, comment);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Clean up
            await _repository.DeleteComment(docRef.Id);
        }

        [Fact]
        public async Task UpdateComment_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var comment = new Comment { Id = "mismatched-id", Body = "This is a test comment", PostId = "post1", UserId = "user1" };

            // Act
            var result = await _controller.UpdateComment("different-id", comment);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateComment_ReturnsNotFoundResult_WhenCommentNotFound()
        {
            // Arrange
            var comment = new Comment { Id = "non-existent-id", Body = "This is a test comment", PostId = "post1", UserId = "user1" };

            // Act
            var result = await _controller.UpdateComment(comment.Id, comment);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetCommentById_ReturnsOkResult_WithComment()
        {
            // Arrange
            var comment = new Comment { Body = "This is a test comment", PostId = "post1", UserId = "user1" };
            var docRef = await _repository.Add(comment);

            // Act
            var result = await _controller.GetCommentById(docRef.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Comment>(okResult.Value);
            Assert.Equal(comment.Body, returnValue.Body);

            // Clean up
            await _repository.DeleteComment(docRef.Id);
        }

        [Fact]
        public async Task GetCommentById_ReturnsNotFoundResult_WhenCommentNotFound()
        {
            // Act
            var result = await _controller.GetCommentById("non-existent-id");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
