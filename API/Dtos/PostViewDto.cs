using API.Models;

namespace API.Dtos
{
    public class PostViewDto
    {
        public int? PostViewId { get; set; }

        public int? PersonId { get; set; }

        public int? PostId { get; set; }

        public virtual Person Person { get; set; }

        public virtual Post Post { get; set; }
    }
}
