using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories.Base;

namespace ASM.Core.Repositories
{
    public interface IAccessGroupModulePermissionRepository : IRepository<AccessGroupModulePermission>
    {
        Task<IEnumerable<AccessGroupModulePermission>> GetAccessGroupModulePermission();
        Task<IEnumerable<AccessGroupModulePermission>> GetAccessGroupModulePermissionMenu();
        Task<IEnumerable<AccessGroupModulePermission>> IsAccessGroupModulePermissionExits(int moduleId);
    }
}