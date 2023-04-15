using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IStatsRepository
    {
        bool AddView(PostViewDto post);
        void AddVote(PostCommentReaction vote);
        void RemoveVote(PostCommentReaction vote);
        bool SaveAll();
    }
}
