using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class QuestionSolutionController:BaseAPIController
    {
        private readonly IQuestionSolutionRepository _repository;

        public QuestionSolutionController(IQuestionSolutionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("add/{internId:int}/{testId:int}")]
        public ActionResult EditInternIntoGroups([FromBody] List<Answer> answers, int internId, int testId)
        {
            ReceiveAnswersDto answersDto = new ReceiveAnswersDto
            {
                InternId = internId,
                TestId = testId,
                Answers = answers
            };
            _repository.Create(answersDto);
            return _repository.SaveAll()? Ok() : BadRequest("Internal Server Error");
        }

    }
}
