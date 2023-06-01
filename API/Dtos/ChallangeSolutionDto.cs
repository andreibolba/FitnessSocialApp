using API.Models;

namespace API.Dtos
{
    public class ChallangeSolutionDto
    {
        public int ChallangeSolutionId { get; set; }

        public int ChallangeId { get; set; }

        public DateTime? DateOfSolution { get; set; }

        public int InternId { get; set; }

        public string SolutionLink { get; set; }

        public ChallangeDto Challange { get; set; }

        public PersonDto Intern { get; set; }
    }
}
