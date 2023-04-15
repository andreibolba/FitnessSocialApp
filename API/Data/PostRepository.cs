using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public PostRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreatePost(PostDto post)
        {
            var postToAdd = _mapper.Map<Post>(post);
            postToAdd.DateOfPost = DateTime.Now;
            _context.Posts.Add(postToAdd);
        }

        public bool DeletePost(int postId)
        {
            var post = _mapper.Map<Post>(GetPost(postId));
            if (post == null)
                return false;
            post.Deleted = true;
            _context.Posts.Update(post);
            return true;
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var allPosts = _context.Posts.Where(p => p.Deleted == false)
                .OrderBy(p => p.DateOfPost)
                .Include(p=>p.Person);
            var allPostsToSend = _mapper.Map<IEnumerable<PostDto>>(allPosts);
            foreach (var a in allPostsToSend)
            {
                var likes = _context.PostCommentReactions.Where(p=>
                p.Deleted==false
                && p.PostId!=null
                && p.PostId==a.PostId
                && p.CommentId==null
                && p.Upvote!=null
                && p.DoenVote==null).Count();
                var dislikes = _context.PostCommentReactions.Where(p =>
                p.Deleted == false
                && p.PostId != null
                && p.PostId == a.PostId
                && p.CommentId == null
                && p.Upvote == null
                && p.DoenVote != null).Count();
                a.Karma = likes - dislikes;
                a.Views = _context.PostViews.Where(pw=>pw.Deleted==false && pw.PostId == a.PostId).Count();
            }
            return allPostsToSend;
        }

        public PostDto GetPost(int postId)
        {
            return GetAllPosts().SingleOrDefault(p => p.PostId == postId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public bool UpdatePost(PostDto post)
        {
            var postToUpdate = GetPost(post.PostId.Value);
            if (postToUpdate == null)
                return false;
            postToUpdate.Title = post.Title == null ? postToUpdate.Title : post.Title;
            postToUpdate.Content = post.Content == null ? postToUpdate.Content : post.Content;
            _context.Posts.Update(_mapper.Map<Post>(postToUpdate));
            return true;
        }
    }
}
