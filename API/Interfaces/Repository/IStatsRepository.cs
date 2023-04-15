using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IStatsRepository
    {
        bool AddView(PostViewDto post);
        void Vote(PostCommentReactionDto vote);
        bool SaveAll();
    }
}
