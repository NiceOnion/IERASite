using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Comments.Models
{
    public class CommentRepository : ICommentRepository
    {
        private readonly FirestoreDb _db;

        public CommentRepository()
        {
            _db = FirestoreDb.Create("ierasite-d02be");
        }

        public async Task<DocumentReference> Add(Comment comment)
        {
            CollectionReference colRef = _db.Collection("Comments");
            DocumentReference docRef = await colRef.AddAsync(comment);
            return docRef;
        }

        public async Task<bool> DeleteAnnouncement(string id)
        {
            DocumentReference docRef = _db.Collection("Comments").Document(id);
            await docRef.DeleteAsync();
            return true;
        }

        public async Task<List<Comment>> GetAllCommentsFromPost(string postId)
        {
            CollectionReference colRef = _db.Collection("Comments");
            Query query = colRef.WhereEqualTo("PostId", postId);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            List<Comment> comments = new List<Comment>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Comment comment = document.ConvertTo<Comment>();
                    comments.Add(comment);
                }
            }
            return comments;
        }

        public async Task<List<Comment>> GetAllCommentsFromUser(string userId)
        {
            CollectionReference colRef = _db.Collection("Comments");
            Query query = colRef.WhereEqualTo("UserId", userId);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            List<Comment> comments = new List<Comment>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Comment comment = document.ConvertTo<Comment>();
                    comments.Add(comment);
                }
            }
            return comments;
        }

        public async Task<bool> UpdateAnnouncement(Comment comment)
        {
            DocumentReference docRef = _db.Collection("Comments").Document(comment.Id);
            await docRef.SetAsync(comment, SetOptions.Overwrite);
            return true;
        }
    }
}
