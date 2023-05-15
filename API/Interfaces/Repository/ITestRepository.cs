using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface ITestRepository
    {
        TestDto Create(TestDto test);
        IEnumerable<TestDto> GetAllTests();
        TestDto GetTestById(int id);
        IEnumerable<TestDto> GetTestByTrainerIdId(int trainerId);
        int GetTestForGroup(int groupId);
        void Update(TestDto test);
        void Delete(int test);
        void StopEdit(int id);
        bool SaveAll();
    }
}