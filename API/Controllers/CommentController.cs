using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class CommentController : BaseAPIController
    {
        private readonly ICommentRepository _comment;
        private readonly IStatsRepository _stats;

        public CommentController(ICommentRepository comment, IStatsRepository stats)
        {
            _comment = comment;
            _stats = stats;
        }

        [HttpGet]
        public ActionResult GetAllComment()
        {
            return Ok(_comment.GetAllComm());
        }

        [HttpGet("post/{postId:int}/{personId:int}")]
        public ActionResult GetAllCommentByPostAndPersonId(int postId, int personId)
        {
            return Ok(_comment.GetAllCommByPostId(postId,personId));
        }

        [HttpPost("add")]
        public ActionResult AddComment([FromBody] CommentDto comment) {
            if(comment.PersonId == null
                || comment.PostId == null
                || comment.CommentContent == null)
                return BadRequest("There are some empty fields!");
            var res = _comment.AddComm(comment);
            return _comment.SaveAll() ? Ok(res) : BadRequest("Internal Server error");
        }

        [HttpPost("edit")]
        public ActionResult EditComment([FromBody] CommentDto comment)
        {
            if (comment.PersonId == null
                || comment.PostId == null
                || comment.DateOfComment == null
                || comment.CommentContent == null
                || comment.CommentId == null )
                return BadRequest("There are some empty fields!");
            var res = _comment.UpdateComm(comment);
            return _comment.SaveAll() ? Ok(res) : BadRequest("Internal Server error");
        }

        [HttpPost("delete/{commId:int}")]
        public ActionResult DeleteComment(int commId)
        {
            _comment.DeleteComm(commId);
            return _comment.SaveAll() ? Ok() : BadRequest("Internal server error");
        }

        [HttpPost("vote")]
        public ActionResult VoteComment([FromBody] PostCommentReactionDto reaction)
        {
            if (reaction.PersonId == null
                || reaction.CommentId == null
                || reaction.Upvote == null
                || reaction.Downvote == null)
                return BadRequest("There are some empty fields!");
            _stats.Vote(reaction);
            return _stats.SaveAll() ? Ok() : BadRequest("Internal server error");
        }
    }
}
