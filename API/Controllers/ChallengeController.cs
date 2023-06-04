using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ChallengeController :BaseAPIController
    {
        private readonly IChallengeRepository _challangeRepository;
        private readonly IChallengeSolutionRepository _challangeSolutionRepository;
        private readonly IPersonRepository _personRepository;

        public ChallengeController(IChallengeRepository challangeRepository, IChallengeSolutionRepository challangeSolutionRepository, IPersonRepository personRepository)
        {
            _challangeRepository = challangeRepository;
            _challangeSolutionRepository = challangeSolutionRepository;
            _personRepository = personRepository;
        }

        [HttpGet()]
        public ActionResult GetAllChallenges()
        {
            var challenges = _challangeRepository.GetAllChallenges();
            return Ok(_challangeRepository.GetAllChallenges());
        }

        [HttpPost("people")]
        public ActionResult GetPoeple([FromBody] Person person) 
        
        {
            return Ok();
        }


        [HttpPost("add")]
        public ActionResult AddChallenge([FromBody] ChallengeDto challenge)
        {
            var isChallange = _challangeRepository.ExistsChallengeForSpecificDate(challenge.Deadline.Year,challenge.Deadline.Month,challenge.Deadline.Day);
            if (isChallange)
                return BadRequest("Is one challange with this dealine");
            if (challenge.Deadline < DateTime.Now)
                return BadRequest("You can't have a challenge with a dealine that is overdue!");
            var res = _challangeRepository.CreateChallenge(challenge);

            return res!=null ? Ok(res): BadRequest("Internal Server Error");
        }

        [HttpPost("edit")]
        public ActionResult EditChallenge([FromBody] ChallengeDto challenge)
        {
            var isChallange = _challangeRepository.ExistsChallengeForSpecificDate(challenge.Deadline.Year, challenge.Deadline.Month, challenge.Deadline.Day);
            if (isChallange)
                return BadRequest("Is one challange with this dealine");
            if (challenge.Deadline < DateTime.Now)
                return BadRequest("You can't have a challenge with a dealine that is overdue!");
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

        [HttpPost("solutions/aprrove/{solutionId}/{points}")]
        public ActionResult ApproveSolution(int solutionId, int points)
        {
            _challangeSolutionRepository.ApproveDeclineSolutin(solutionId, true,points);
            return _challangeSolutionRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("solutions/decline/{solutionId}")]
        public ActionResult DeclineSolution(int solutionId)
        {
            _challangeSolutionRepository.ApproveDeclineSolutin(solutionId, false, 0);
            return _challangeSolutionRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("solutions/add/{personId}/{challengeId}")]
        public ActionResult AddSolution(int personId,int challengeId)
        {
            var challenge = _challangeRepository.GetChallengeById(challengeId);
            if (DateTime.Now > challenge.Deadline)
                return BadRequest("Deadline is overdue!");

            IFormFile file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No file received!");

            long fileSize = file.Length;

            if (fileSize > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] zipFile = stream.ToArray();

                    var challengeSolution = new ChallengeSolutionDto();
                    challengeSolution.ChallangeId = challengeId;
                    challengeSolution.InternId = personId;
                    challengeSolution.SolutionFile = zipFile;

                    var res = _challangeSolutionRepository.CreateSolution(challengeSolution);

                    return res != null ? Ok(res) : BadRequest("Internal Server Error");
                }
            }
            return BadRequest("Internal Server Error");
        }

        [HttpPost("solutions/edit")]
        public ActionResult EditSolution([FromBody] ChallengeSolutionDto challengeSolution)
        {
            var challenge = _challangeRepository.GetChallengeById(challengeSolution.ChallangeId);
            if (DateTime.Now > challenge.Deadline)
                return BadRequest("Deadline is overdue!");
            var res = _challangeSolutionRepository.UpdateSolution(challengeSolution);

            return res != null ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("solutions/delete/{solutionId}")]
        public ActionResult DeleteSolution(int solutionId)
        {
            _challangeSolutionRepository.DeleteSolution(solutionId);

            return _challangeSolutionRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpGet("solutions/download/{solutionChallangeId}")]
        public ActionResult GetSolutionFile(int solutionChallangeId)
        {
            var sol = _challangeSolutionRepository.GetSolutionById(solutionChallangeId);

            string contentType = "application/octet-stream";
            string fileName = "attempt_"+sol.Intern.Username+".zip";

            return File(sol.SolutionFile, contentType, fileName);

        }
    }
}
