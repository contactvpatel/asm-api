using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Business.Models;

namespace ASM.Business.Interfaces
{
    public interface IAccessGroupAssignmentService
    {
        Task<IEnumerable<AccessGroupAssignmentModel>> GetAll();
        Task Create(IEnumerable<AccessGroupAssignmentModel> accessGroupModel);
        Task Delete(int id, int userId);
    }
}
