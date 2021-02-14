using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Business.Models;

namespace ASM.Business.Interfaces
{
    public interface IAccessGroupService
    {
        Task<IEnumerable<AccessGroupModel>> GetAll();
        Task<AccessGroupModel> GetById(int id);
        Task<IEnumerable<AccessGroupModel>> GetByApplicationDepartment(Guid applicationId, int departmentId);
        Task<AccessGroupModel> Create(AccessGroupModel accessGroupModel);
        Task<AccessGroupModel> Update(AccessGroupModel accessGroupModel);
        Task Delete(int id, int userId);
        Task<bool> IsAccessGroupExists(AccessGroupModel module);
    }
}
