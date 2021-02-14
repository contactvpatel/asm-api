using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories.Base;

namespace ASM.Core.Repositories
{
    public interface IAccessGroupRepository : IRepository<AccessGroup>
    {
        Task<IEnumerable<AccessGroup>> GetAll();
        Task<AccessGroup> GetById(int id);
        Task<IEnumerable<AccessGroup>> GetByApplicationDepartment(Guid applicationId, int departmentId);
        Task<AccessGroup> Update(AccessGroup accessGroup);
        Task Delete(int id, int userId);
        Task<bool> IsAccessGroupExists(AccessGroup accessGroups);
    }
}
