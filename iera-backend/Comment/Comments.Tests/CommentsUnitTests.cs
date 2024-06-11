using Xunit;
using Moq;
using Comments.Controllers;
using Comments.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

public class CommentControllerUnitTests
{
    private readonly Mock<ICommentRepository> _mockRepo;
    private readonly CommentController _controller;

    public CommentControllerUnitTests()
    {
        _mockRepo = new Mock<ICommentRepository>();
        _controller = new CommentController(_mockRepo.Object);
    }

    //[Fact]
    //public async Task AddComment_ReturnsCreatedAtAction()
    //{
    //    // Arrange
    //    var newComment = new Comment
    //    {
    //        Id = "1",
    //        PostId = "post1",
    //        UserID = "user1",
    //        Body = "Test comment",
    //        PostTime = Timestamp.FromDateTime(DateTime.UtcNow)
    //    };

    //    var mockRepo = new Mock<ICommentRepository>();
    //    mockRepo.Setup(repo => repo.Add(It.IsAny<Comment>())).ReturnsAsync(newComment);

    //    var _controller = new CommentController(mockRepo.Object);

    //    // Act
    //    var result = await _controller.AddComment(newComment);

    //    // Assert
    //    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
    //    Assert.Equal("GetCommentById", createdAtActionResult.ActionName);
    //}


    [Fact]
    public async Task DeleteComment_ReturnsNoContent()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.DeleteComment("1")).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteComment("1");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetCommentsByPost_ReturnsOk()
    {
        // Arrange
        var comments = new List<Comment> { new Comment { Id = "1", PostId = "post1", UserId = "user1", Body = "Test comment", PostTime = Timestamp.FromDateTime( DateTime.UtcNow) } };
        _mockRepo.Setup(repo => repo.GetAllCommentsFromPost("post1")).ReturnsAsync(comments);

        // Act
        var result = await _controller.GetCommentsByPost("post1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnComments = Assert.IsType<List<Comment>>(okResult.Value);
        Assert.Single(returnComments);
    }

    [Fact]
    public async Task GetCommentsByUser_ReturnsNotFound_WhenNoComments()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllCommentsFromUser("user1")).ReturnsAsync((List<Comment>)null);

        // Act
        var result = await _controller.GetCommentsByUser("user1");

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateComment_ReturnsNoContent()
    {
        // Arrange
        var comment = new Comment { Id = "1", PostId = "post1", UserId = "user1", Body = "Updated comment", PostTime = Timestamp.FromDateTime( DateTime.UtcNow )};
        _mockRepo.Setup(repo => repo.UpdateComment(comment)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateComment("1", comment);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
