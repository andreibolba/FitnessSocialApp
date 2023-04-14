using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
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

            _context.Posts.Add(postToAdd);
        }

        public bool DeletePost(int postId)
        {
            var post = GetPost(postId);
            if (post == null)
                return false;
            post.Deleted = true;
            return true;
        }

        public IEnumerable<PostDto> GetAllPosts()
        {
            var allPosts = _context.Posts.Where(p => p.Deleted == false).OrderBy(p => p.DateOfPost);
            return _mapper.Map<IEnumerable<PostDto>>(allPosts);
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
            var postToUpdate = GetPost(post.PostId);
            if (postToUpdate == null)
                return false;
            postToUpdate.Content = post.Content == null ? postToUpdate.Content : post.Content;
            _context.Posts.Update(_mapper.Map<Post>(postToUpdate));
            return true;
        }
    }
}
