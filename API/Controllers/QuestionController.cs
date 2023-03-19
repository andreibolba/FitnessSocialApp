using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    public class QuestionController : BaseAPIController
    {
        private readonly IQuestionRepository _question;

        public QuestionController(IQuestionRepository question)
        {
            _question = question;
        }

        [HttpGet]
        public ActionResult<IEnumerable<QuestionDto>> GetAllQuestions()
        {
            return Ok(_question.GetAllQuestions());
        }

        [HttpGet("{questionId:int}")]
        public ActionResult<IEnumerable<QuestionDto>> GetQuetionById(int questionId)
        {
            return Ok(_question.GetQuestionById(questionId));
        }

        [HttpGet("trainers/{trainerId:int}")]
        public ActionResult<IEnumerable<QuestionDto>> GetQuetionByTrainerId(int trainerId)
        {
            return Ok(_question.GetAllQuestionsByTrainerId(trainerId));
        }


        [HttpPost("add")]
        public ActionResult AddQuestion([FromBody] QuestionDto question)
        {
            _question.Create(question);
            return _question.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{questionId:int}")]
        public ActionResult DeleteQuestion(int questionId)
        {
            _question.Delete(questionId);
            return _question.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("stop/{questionId:int}")]
        public ActionResult StopEditQuestion(int questionId)
        {
            _question.StopEdit(questionId);
            return _question.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult UpdateQuestion([FromBody] QuestionDto question)
        {
            _question.Update(question);
            return _question.SaveAll() ? Ok() : BadRequest("Internal Server Error"); ;
        }
    }
}
