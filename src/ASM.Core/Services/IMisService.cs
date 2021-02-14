using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;

namespace ASM.Core.Services
{
    public interface IMisService
    {
        Task<IEnumerable<Department>> GetAllDepartments();
        Task<Department> GetDepartmentById(int id);
        Task<IEnumerable<Role>> GetAllRoles();
        Task<IEnumerable<Role>> GetRoleByDepartmentId(int departmentId);
        Task<IEnumerable<Position>> GetPositions(int roleId);
    }
}
