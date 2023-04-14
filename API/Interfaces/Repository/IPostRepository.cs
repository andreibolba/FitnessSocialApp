using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IPostRepository
    {
        void CreatePost(PostDto post);
        void UpdatePost(PostDto post);
        void DeletePost(int postId);
        IEnumerable<PostDto> GetAllPosts();
        PostDto GetPost(int postId);
        bool SaveAll();
    }
}
