using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostsController : BaseAPIController
    {
        private readonly IPostRepository _post;

        public PostsController(IPostRepository post)
        {
            _post = post;
        }

        [HttpGet]
        public ActionResult GetAllMeetings()
        {
            return Ok(_post.GetAllPosts());
        }

        [HttpGet("{postId:int}")]
        public ActionResult GetOneMeeting(int postId)
        {
            return Ok(_post.GetPost(postId));
        }

        [HttpPost("add")]
        public ActionResult AddPost([FromBody]PostDto post)
        {
            if (post.PersonId == null
                || post.Content == null
                || post.DateOfPost == null)
                return BadRequest("There are some empty fields!");
            _post.CreatePost(post);
            return Ok(_post.SaveAll());
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
                || post.Content == null
                || post.DateOfPost == null
                || post.PostId == null)
                return BadRequest("There are some empty fields!");
            _post.UpdatePost(post);
            return Ok(_post.SaveAll());
        }

    }
}
