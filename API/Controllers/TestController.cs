using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TestController : BaseAPIController
    {
        private readonly ITestRepository _test;

        public TestController(ITestRepository test)
        {
            _test = test;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TestDto>> GetAllTests()
        {
            return Ok(_test.GetAllTests());
        }

        [HttpGet("{testId:int}")]
        public ActionResult<IEnumerable<TestDto>> GetTestById(int testId)
        {
            return Ok(_test.GetTestById(testId));
        }

        [HttpGet("mytest/{trainerId:int}")]
        public ActionResult<IEnumerable<TestDto>> GetTestByTrainerId(int trainerId)
        {
            return Ok(_test.GetTestByTrainerIdId(trainerId));
        }


        [HttpPost("add")]
        public ActionResult AddTest([FromBody] TestDto test)
        {
            _test.Create(test);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{testId:int}")]
        public ActionResult DeleteTest(int testId)
        {
           _test.Delete(testId);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("stop/{testId:int}")]
        public ActionResult StopEditTest(int testId)
        {
            _test.StopEdit(testId);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult UpdateQuestion([FromBody] TestDto test)
        {
            _test.Update(test);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error"); ;
        }

        [HttpPost("addquestion/{questionId:int}")]
        public ActionResult EditQuestion([FromBody] TestDto test,int questionId)
        {
            _test.AddQuestionToTest(test.TestId, questionId);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("testattribution")]
        public ActionResult EditTestsToInternsOrGroups([FromBody] TestGroupInternDto test)
        {
            if (test.InternId != null)
            {
                if (test.IsDelete == false)
                _test.AddTestToStudent(test.TestId, test.InternId.Value);
                else
                    _test.RemoveTestFromStudents(test.TestId, test.InternId.Value);
            }
            if (test.GroupId != null)
            {
                if (test.IsDelete == false)
                    _test.AddTestToGroup(test.TestId, test.GroupId.Value);
                else
                    _test.RemoveTestFromGruop(test.TestId, test.GroupId.Value);
            }
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

    }
}
