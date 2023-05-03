using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IPostRepository
    {
        void CreatePost(PostDto post);
        bool UpdatePost(PostDto post);
        bool DeletePost(int postId);
        IEnumerable<PostDto> GetAllPostsByPersonId(int personId);
        IEnumerable<PostDto> GetAllPosts();
        PostDto GetPost(int postId);
        bool SaveAll();
    }
}
