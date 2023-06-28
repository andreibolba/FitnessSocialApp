using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IPostRepository
    {
        PostDto CreatePost(PostDto post);
        PostDto UpdatePost(PostDto post);
        bool DeletePost(int postId);
        IEnumerable<PostDto> GetAllPostsByPersonId(int personId);
        IEnumerable<PostDto> GetAllPosts();
        PostDto GetPost(int postId);
        bool SaveAll();
    }
}
