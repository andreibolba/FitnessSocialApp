using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IPersonRepository _person;
        private readonly IMapper _mapper;

        public CommentRepository(InternShipAppSystemContext context, IPersonRepository person, IMapper mapper)
        {
            _context = context;
            _person = person;
            _mapper = mapper;
        }

        public CommentDto AddComm(CommentDto comment)
        {
            var comm = _mapper.Map<Comment>(comment);
            comm.DateOfComment = DateTime.Now;
            _context.Comments.Add(comm);
            return SaveAll() ? _mapper.Map<CommentDto>(comm) : null;
        }

        public void DeleteComm(int commId)
        {
            var comm = _mapper.Map<Comment>(GetCommById(commId));
            comm.Deleted = true;
            _context.Comments.Update(comm);            
        }

        public IEnumerable<CommentDto> GetAllCommForPersonId(int personId)
        {
            var allComms = _context.Comments.Where(c => c.Deleted == false).Include(c => c.Post);
            var comms = _mapper.Map<IEnumerable<CommentDto>>(allComms);
            foreach(var comm in comms)
            {
                comm.Person = _person.GetPersonById(comm.PersonId.Value);
                var isUp = _context.PostCommentReactions.SingleOrDefault(p => p.CommentId == comm.CommentId && p.PersonId == personId && p.Deleted == false);
                if (isUp == null)
                {
                    comm.Upvote = false;
                    comm.Downvote = false;
                }
                else
                {
                    comm.Upvote = isUp.Upvote.Value;
                    comm.Downvote = isUp.DownVote.Value;

                }
                var likes = _context.PostCommentReactions.Where(p =>
                p.Deleted == false
                && p.PostId == null
                && p.CommentId != null
                && p.CommentId == comm.CommentId
                && p.Upvote == true
                && p.DownVote == false).Count();
                var dislikes = _context.PostCommentReactions.Where(p =>
                p.Deleted == false
                && p.PostId == null
                && p.CommentId != null
                && p.CommentId == comm.CommentId
                && p.Upvote == false
                && p.DownVote == true).Count();
                comm.Karma = likes - dislikes;
            }
            return comms;
        }

        public IEnumerable<CommentDto> GetAllComm()
        {
            var allComms = _context.Comments.Where(c => c.Deleted == false).Include(c => c.Post).Include(c=>c.Person);
            var comms = _mapper.Map<IEnumerable<CommentDto>>(allComms);
            return comms;
        }

        public IEnumerable<CommentDto> GetAllCommByPostId(int postId, int personId)
        {
            return GetAllCommForPersonId(personId).Where(c => c.PostId == postId);
        }

        public CommentDto GetCommById(int commId)
        {
            return GetAllComm().SingleOrDefault(c=>c.CommentId == commId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public CommentDto UpdateComm(CommentDto comment)
        {
            var commFromDb = _mapper.Map<Comment>(GetCommById(comment.CommentId.Value));
            commFromDb.CommentContent = comment.CommentContent == null ? commFromDb.CommentContent : comment.CommentContent;
            _context.Comments.Update(commFromDb);
            return SaveAll() ? _mapper.Map<CommentDto>(commFromDb) : null;
        }
    }
}
