using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class StatsRepository : IStatsRepository
    {
        private InternShipAppSystemContext _context;
        private IMapper _mapper;

        public StatsRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool AddView(PostViewDto post)
        {
            var view = _context.PostViews.SingleOrDefault(v => v.PersonId == post.PersonId && v.PostId == post.PostId);
            if (view == null)
            {
                var postToAdd = _mapper.Map<PostView>(post);
                postToAdd.Deleted = false;
                _context.PostViews.Add(postToAdd);
                return true;
            }
            return false;
        }

        public void AddVote(PostCommentReaction vote)
        {
            var reaction = _context.PostCommentReactions.SingleOrDefault(v=>
            v.PersonId == vote.PersonId 
            && (v.PostId == vote.PostId || v.CommentId == vote.CommentId));
            if(reaction != null)
            {
                reaction.Deleted = false;
                _context.PostCommentReactions.Update(reaction);
                return;
            }
            
            _context.PostCommentReactions.Add(vote);
        }

        public void RemoveVote(PostCommentReaction vote)
        {
            var reaction = _context.PostCommentReactions.SingleOrDefault(v =>
            v.PersonId == vote.PersonId
            && (v.PostId == vote.PostId || v.CommentId == vote.CommentId));
            if (reaction != null)
            {
                reaction.Deleted = true;
                _context.PostCommentReactions.Update(reaction);
                return;
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
