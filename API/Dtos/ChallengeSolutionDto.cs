﻿using API.Models;

namespace API.Dtos
{
    public class ChallengeSolutionDto
    {
        public int ChallangeSolutionId { get; set; }

        public int ChallangeId { get; set; }

        public DateTime DateOfSolution { get; set; }

        public int InternId { get; set; }

        public byte[] SolutionFile { get; set; }

        public int Points { get; set; }

        public bool? Approved { get; set; }

        public ChallengeDto Challange { get; set; }

        public PersonDto Intern { get; set; }
    }
}
