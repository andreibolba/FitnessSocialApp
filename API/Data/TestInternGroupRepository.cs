using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TestInternGroupRepository : ITestInternGroupRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public TestInternGroupRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //op-1=>intern
        //op-2=>group
        public bool UpdateAllTestInternGroup(string obj, int testId, int op)
        {
            var result = op == 1
               ? _context.TestGroupInterns.Where(t => t.Deleted == false && t.TestId == testId && t.GroupId == null)
               : _context.TestGroupInterns.Where(t => t.Deleted == false && t.TestId == testId && t.InternId == null);
            List<int> idList = Utils.Utils.FromStringToInt(obj);
            if (result.Count() == 0 && idList.Count() == 0)
                return false;
            foreach (var res in result)
            {
                int objId = op == 1 ? res.InternId.Value : res.GroupId.Value;
                bool delete = idList.IndexOf(objId) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    idList.Remove(objId);
                _context.TestGroupInterns.Update(res);
            }

            foreach (var id in idList)
            {
                _context.TestGroupInterns.Add(new TestGroupIntern
                {
                    TestId = testId,
                    GroupId = op == 2 ? id : null,
                    InternId = op == 1 ? id : null,
                    Deleted = false
                });
            }
            return true;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<T> GettAllChecked<T>(int testId)
        {
            var response = _context.TestGroupInterns.Where(tgi => tgi.Deleted == false && tgi.TestId == testId);
            if (typeof(T) == typeof(TestInternDto))
            {
               var interns = response
                    .Where(r=>r.InternId!=null)
                    .Include(tgi => tgi.Intern)
                    .Select(tgi => new TestInternDto
                    {
                        InternId = tgi.InternId.Value,
                        FirstName = tgi.Intern.FirstName,
                        LastName = tgi.Intern.LastName,
                        Username = tgi.Intern.Username,
                        IsChecked = true
                    }).ToList();
                    foreach (var intern in _mapper.Map<IEnumerable<PersonDto>>(_context.People.Where(p => p.Deleted == false && p.Status == "Intern")))
                    {
                        if (interns.FirstOrDefault(i => i.InternId == intern.PersonId) == null)
                        {
                            interns.Add(new TestInternDto
                            {
                                InternId = intern.PersonId,
                                FirstName = intern.FirstName,
                                LastName = intern.LastName,
                                Username = intern.Username,
                                IsChecked = false
                            });
                        }
                    }
                    return (IEnumerable<T>)interns;
            }
            else if (typeof(T) == typeof(TestGroupDto))
            {
                var groups = response
                        .Where(r=>r.GroupId!=null)
                        .Include(tgi => tgi.Group)
                        .Select(tgi => new TestGroupDto
                        {
                            GroupId = tgi.GroupId.Value,
                            GroupName = tgi.Group.GroupName,
                            IsChecked = true
                        }).ToList();
                    foreach (var gr in _mapper.Map<IEnumerable<GroupDto>>(_context.Groups.Where(p => p.Deleted == false)))
                    {
                        if (groups.FirstOrDefault(i => i.GroupId == gr.GroupId) == null)
                        {
                            groups.Add(new TestGroupDto
                            {
                                GroupId = gr.GroupId,
                                GroupName = gr.GroupName,
                                IsChecked = false
                            });
                        }
                    }
                    return (IEnumerable<T>)groups;
            }
            return null;
        }
    }
}
