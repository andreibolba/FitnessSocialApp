using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Authorize]
    public class TestController : BaseAPIController
    {
        private readonly ITestRepository _test;
        private readonly ITestQuestionRepository _testQuestion;
        private readonly ITestInternGroupRepository _testInternGroup;

        public TestController(ITestRepository test, ITestQuestionRepository testQuestion, ITestInternGroupRepository testInternGroup)
        {
            _test = test;
            _testQuestion = testQuestion;
            _testInternGroup = testInternGroup;
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

        [HttpGet("mytest/{id:int}/{status}")]
        public ActionResult<IEnumerable<TestDto>> GetTestByTrainerId(int id,string status)
        {
            if(status=="trainer")
                return Ok(_test.GetTestByTrainerId(id));
            return Ok(_test.GetInternTest(id));
        }


        [HttpPost("add")]
        public ActionResult AddTest([FromBody] TestDto test)
        {
            var res=_test.Create(test);
            return res!=new TestDto() ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{questionId:int}")]
        public ActionResult DeleteTest(int questionId)
        {
            _test.Delete(questionId);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("stop/{testId:int}")]
        public ActionResult StopTest(int testId)
        {
            _test.StopEdit(testId);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult Update([FromBody] TestDto test)
        {
            _test.Update(test);
            return _test.SaveAll() ? Ok() : BadRequest("Internal Server Error"); ;
        }


        [HttpGet("unselected/{testId:int}")]
        public ActionResult GetAllUnselectedQuestons(int testId)
        {
            return Ok(_testQuestion.GetUnselectedQuestions(testId));
        }

        [HttpGet("results/people/{testId:int}")]
        public ActionResult GetPeopleRezolvingTest(int testId)
        {
            return Ok(_testQuestion.GetAllPersonResolvingTest(testId));
        }


        [HttpPost("testattribution/update/{testId:int}/{option}")]
        public ActionResult UpdateTestAttributions([FromBody] object ids, int testId,string option)
        {
            Dictionary<string,string> idsData = JsonConvert.DeserializeObject<Dictionary<string,string>>(ids.ToString());
            bool hasSomethingToSave;
            switch (option)
            {
                case "tests":
                    hasSomethingToSave = _testQuestion.UpdateAllQuestions(idsData["ids"] += "!", testId);
                    if (hasSomethingToSave == false)
                        return Ok();
                    return _testQuestion.SaveAll() ? Ok() : BadRequest("Internal server error!");
                case "interns":
                    hasSomethingToSave = _testInternGroup.UpdateAllTestInternGroup(idsData["ids"] += "!", testId,1);
                    if (hasSomethingToSave == false)
                        return Ok();
                    return _testInternGroup.SaveAll() ? Ok() : BadRequest("Internal server error!");
                case "groups":
                    hasSomethingToSave = _testInternGroup.UpdateAllTestInternGroup(idsData["ids"] += "!", testId,2);
                    if (hasSomethingToSave == false)
                        return Ok();
                    return _testInternGroup.SaveAll() ? Ok() : BadRequest("Internal server error!");
                default:
                    return BadRequest("Invalid option!");

            }
        }

        [HttpGet("all/{testId:int}/{option}")]
        public ActionResult UpdateTestAttributions(int testId, string option)
        {
            switch(option)
            {
                case "interns":
                    return Ok(_testInternGroup.GettAllChecked<ObjectInternDto>(testId));
                case "groups":
                    return Ok(_testInternGroup.GettAllChecked<ObjectGroupDto>(testId));
                default:
                    return BadRequest("Invalid option!");
            }
        }

    }
}
