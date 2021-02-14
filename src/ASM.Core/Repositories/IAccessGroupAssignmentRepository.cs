using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories.Base;

namespace ASM.Core.Repositories
{
    public interface IAccessGroupAssignmentRepository : IRepository<AccessGroupAssignment>
    {
        Task<IEnumerable<AccessGroupAssignment>> GetAll();
        Task Create(IEnumerable<AccessGroupAssignment> accessGroupModel);
    }
}
