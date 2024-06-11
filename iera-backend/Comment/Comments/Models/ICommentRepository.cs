using Google.Cloud.Firestore;

namespace Comments.Models
{
    public interface ICommentRepository
    {
        Task<DocumentReference> Add(Comment comment);
        Task<bool> DeleteComment(string id);
        Task<List<Comment>> GetAllCommentsFromPost(string postId);
        Task<List<Comment>> GetAllCommentsFromUser(string userId);
        Task<Comment> GetCommentById(string id);
        Task<bool> UpdateComment(Comment comment);
    }
}
