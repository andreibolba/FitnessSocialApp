using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface ICommentRepository
    {
        CommentDto AddComm(CommentDto comment);
        CommentDto UpdateComm(CommentDto comment);
        void DeleteComm(int commId);
        IEnumerable<CommentDto> GetAllComm();
        IEnumerable<CommentDto> GetAllCommByPostId(int postId, int personId);
        public IEnumerable<CommentDto> GetAllCommForPersonId(int personId);
        CommentDto GetCommById(int commId);
        bool SaveAll();
    }
}
