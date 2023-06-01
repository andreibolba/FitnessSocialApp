using API.Models;

namespace API.Dtos
{
    public class ChallangeDto
    {
        public int ChallangeId { get; set; }

        public string ChallangeName { get; set; }

        public string ChallangeDescription { get; set; }

        public int TrainerId { get; set; }

        public DateTime DateOfPost { get; set; }

        public DateTime Deadline { get; set; }

        public PersonDto Trainer { get; set; }
    }
}
