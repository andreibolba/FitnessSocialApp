using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    public class PostsController : BaseAPIController
    {
        private readonly IPostRepository _post;
        private readonly IStatsRepository _stats;
        private readonly IPersonRepository _personRepository;

        public PostsController(IPostRepository post, IStatsRepository stats, IPersonRepository personRepository)
        {
            _post = post;
            _stats = stats;
            _personRepository = personRepository;
        }

        [HttpGet]
        public ActionResult GetAllPosts()
        {
            return Ok(_post.GetAllPosts());
        }

        [HttpGet("{postId:int}")]
        public ActionResult GetOnePost(int postId)
        {
            return Ok(_post.GetPost(postId));
        }

        [HttpPost("add")]
        public ActionResult AddPost([FromBody]PostDto post)
        {
            if (post.PersonId == null
                || post.Title == null
                || post.Content == null)
                return BadRequest("There are some empty fields!");
            var res = _post.CreatePost(post);
            if(res==null)
                return BadRequest("Internal server error");
            res.Person = _personRepository.GetPersonById(res.PersonId.Value);
            return res!=null ? Ok(res) : BadRequest("Internal server error");
        }


        [HttpPost("delete/{postId:int}")]
        public ActionResult DeletePost(int postId)
        {
            _post.DeletePost(postId);
            return Ok(_post.SaveAll());
        }

        [HttpPost("edit")]
        public ActionResult UpdatePost([FromBody] PostDto post)
        {
            if (post.PersonId == null
                || post.Title == null
                || post.Content == null)
                return BadRequest("There are some empty fields!");
            var res = _post.UpdatePost(post);
            return res!=null ? Ok(res) : BadRequest("Internal server error");
        }

        [HttpPost("view")]
        public ActionResult ViewPost([FromBody] PostViewDto post)
        {
            if (post.PersonId == null
                || post.PostId == null)
                return BadRequest("There are some empty fields!");
            var res = _stats.AddView(post);
            if (res == true)
            {
                return _stats.SaveAll() ? Ok(post) : BadRequest("Internal server error");
            }
            return Ok(null);
        }

        [HttpPost("vote")]
        public ActionResult VotePost([FromBody] PostCommentReactionDto post)
        {
            if (post.PersonId == null
                || post.PostId == null
                || post.Upvote == null
                || post.Downvote == null)
                return BadRequest("There are some empty fields!");
            _stats.Vote(post);
            return _stats.SaveAll() ? Ok() : BadRequest("Internal server error");
        }

    }
}
