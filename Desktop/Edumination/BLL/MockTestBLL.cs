using IELTS.DAL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class MockTestBLL
    {
        private readonly MockTestDAL _dal;
        private readonly TestPaperDAL _paperDal = new();

        public MockTestBLL()
        {
            _dal = new MockTestDAL();
        }

        public List<MockTestDTO> GetAll()
        {
            return _dal.GetAll();
        }

        public MockTestDTO? GetMockTestById(long id)
        {
            return _dal.GetById(id);
        }

        public long Create(MockTestDTO mock)
        {
            if (mock.Year <= 0)
                throw new Exception("Year không hợp lệ");

            if (string.IsNullOrWhiteSpace(mock.Title))
                throw new Exception("Title không được rỗng");

            return _dal.Create(mock);
        }

        public long Update(MockTestDTO mock)
        {
            if (mock.Id <= 0)
                throw new Exception("Id không hợp lệ");

            return _dal.Update(mock);
        }

        public bool Delete(long id)
        {
            return _dal.Delete(id);
        }

        public List<MockTestDTO> GetAllMockTestsWithPapers()
        {
            var mocks = _dal.GetAll();
            var papers = _paperDal.NewGetAll();

            foreach (var m in mocks)
            {
                m.Papers = papers
                    .Where(p => p.MockTestId == m.Id)
                    .OrderBy(p => p.TestMonth)
                    .ToList();
            }

            return mocks;
        }
    }

}
