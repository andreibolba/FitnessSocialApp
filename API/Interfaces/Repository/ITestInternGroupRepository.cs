namespace API.Interfaces.Repository
{
    public interface ITestInternGroupRepository
    {
        bool UpdateAllTestInternGroup(string obj, int testId, int op);
        IEnumerable<T> GettAllChecked<T>(int testId);
        bool SaveAll();
    }
}
