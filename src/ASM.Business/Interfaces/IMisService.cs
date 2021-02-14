using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Util.Models;

namespace ASM.Business.Interfaces
{
    public interface IMisService
    {
        Task<IEnumerable<DepartmentModel>> GetAllDepartments();
        Task<DepartmentModel> GetDepartmentById(int id);
        Task<IEnumerable<RoleModel>> GetAllRoles();
        Task<IEnumerable<RoleModel>> GetRoleByDepartmentId(int departmentId);
        Task<IEnumerable<PositionModel>> GetPositions(int roleId);
    }
}
