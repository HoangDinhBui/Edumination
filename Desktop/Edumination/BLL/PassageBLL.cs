using IELTS.DAL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTS.BLL
{
    public class PassageBLL
    {
        private readonly PassageDAL _dal = new();

        public List<PassageDTO> GetPassagesBySection(long sectionId)
        {
            return _dal.GetBySectionId(sectionId);
        }
    }
}
