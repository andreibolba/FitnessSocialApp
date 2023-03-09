using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface ITestRepository
    {
        void Create(TestDto test);
        IEnumerable<TestDto> GetAllTests();
        IEnumerable<QuestionDto> GettAllQuestionsFromTest(int testId);
        TestDto GetTestById(int id);
        void AddQuestionToTest(int testId, int questionId);
        void RemoveQuestionFromTest(int testId,int questionId);
        void Update(TestDto test);
        void AddTestToStudent(int testId,int internId);
        void AddTestToGroup(int testId,int groupId);
        void RemoveTestFromGruop(int testId,int groupId);
        void RemoveTestFromStudents(int testId,int internId);
        void Delete(int test);
        void StopEdit(int id);
        bool SaveAll();
    }
}