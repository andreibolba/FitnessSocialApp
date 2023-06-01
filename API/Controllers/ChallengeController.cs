using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ChallengeController :BaseAPIController
    {
        private readonly IChallengeRepository _challangeRepository;
        private readonly IChallengeSolutionRepository _challangeSolutionRepository;

        public ChallengeController(IChallengeRepository challangeRepository, IChallengeSolutionRepository challangeSolutionRepository)
        {
            _challangeRepository = challangeRepository;
            _challangeSolutionRepository = challangeSolutionRepository;
        }

        [HttpGet()]
        public ActionResult GetAllChallenges()
        {
            return Ok(_challangeRepository.GetAllChallenges());
        }

        [HttpPost("add")]
        public ActionResult AddChallenge([FromBody] ChallengeDto challenge)
        {
            var res = _challangeRepository.CreateChallenge(challenge);

            return res!=null ? Ok(res): BadRequest("Internal Server Error");
        }

        [HttpPost("edit")]
        public ActionResult EditChallenge([FromBody] ChallengeDto challenge)
        {
            var res = _challangeRepository.UpdateChallenge(challenge);

            return res != null ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{challangeId}")]
        public ActionResult DeleteChallenge(int challangeId)
        {
            _challangeRepository.DeleteChallenge(challangeId);

            return _challangeRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpGet("solutions")]
        public ActionResult GetAllChallengeSolutins()
        {
            return Ok(_challangeSolutionRepository.GetAllSolutions());
        }

        [HttpGet("solutions/{challangeId}")]
        public ActionResult GetAllChallengeSolutinsForChallange(int challangeId)
        {
            return Ok(_challangeSolutionRepository.GetAllSolutionsForChallenge(challangeId));
        }

        [HttpGet("solutions/mine/{personId}")]
        public ActionResult GetAllChallengeSolutinsForIntern(int personId)
        {
            return Ok(_challangeSolutionRepository.GetAllSolutionsForIntern(personId));
        }

        [HttpPost("solutions/{challangeId}/{personId}")]
        public ActionResult ApproveSolution(int challangeId,int personId)
        {
            return Ok();
        }

        [HttpPost("solutions/{challangeId}/{personId}/decline")]
        public ActionResult DeclineSolution(int challangeId, int personId)
        {
            return Ok();
        }

        [HttpPost("solutions/add")]
        public ActionResult AddSolution([FromBody] ChallengeSolutionDto challengeSolution)
        {
            var res = _challangeSolutionRepository.CreateSolution(challengeSolution);

            return res!=null ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("solutions/edit")]
        public ActionResult EditSolution([FromBody] ChallengeSolutionDto challengeSolution)
        {
            var res = _challangeSolutionRepository.UpdateSolution(challengeSolution);

            return res != null ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("solutions/delete/{solutionId}")]
        public ActionResult DeleteSolution(int solutionId)
        {
            _challangeSolutionRepository.DeleteSolution(solutionId);

            return _challangeSolutionRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}
