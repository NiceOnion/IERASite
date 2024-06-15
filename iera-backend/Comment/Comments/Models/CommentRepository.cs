using System.Collections.Generic;
using System.Threading.Tasks;
using Comments.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Comments.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoDBContext _context;
        private readonly IMongoCollection<Comment> _commentsCollection;

        public CommentRepository(IMongoDBContext context)
        {
            _context = context;
            _commentsCollection = _context.Comments;
        }

        public async Task<Comment> GetCommentById(string id)
        {
            return await _commentsCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Comment>> GetAllCommentsFromPost(string postId)
        {
            return await _commentsCollection.Find(c => c.PostId == postId).ToListAsync();
        }

        public async Task<List<Comment>> GetAllCommentsFromUser(string userId)
        {
            return await _commentsCollection.Find(c => c.UserId == userId).ToListAsync();
        }

        public async Task Add(Comment comment)
        {
            comment.Id = ObjectId.GenerateNewId().ToString();
            await _commentsCollection.InsertOneAsync(comment);
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            var result = await _commentsCollection.ReplaceOneAsync(c => c.Id == comment.Id, comment);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async void UpdateCommentDeletedAccount(string userId)
        {
            var filter = Builders<Comment>.Filter.Eq(a => a.UserId, userId);
            var update = Builders<Comment>.Update.Set(a => a.UserId, "User removed");

            var result = await _context.Comments.UpdateManyAsync(filter, update);
        }

        public async Task<bool> DeleteComment(string id)
        {
            var result = await _commentsCollection.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
