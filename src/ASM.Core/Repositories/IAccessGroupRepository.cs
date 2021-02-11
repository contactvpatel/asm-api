using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories.Base;

namespace ASM.Core.Repositories
{
    public interface IAccessGroupRepository : IRepository<AccessGroup>
    {
        Task<IEnumerable<AccessGroup>> GetAccessGroupList();
        Task<AccessGroup> GetAccessGroupById(int id);
        Task<bool> IsAccessGroupExists(AccessGroup accessGroups);
        Task<bool> IsAccessGroupNameExists(int id, string name);
        Task<IEnumerable<AccessGroup>> GetAccessGroupByDepartment(Guid applicationId, int departmentId);
    }
}
