using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Util.Models;

namespace ASM.Core.Services
{
    public interface IMisService
    {
        Task<IEnumerable<DepartmentModel>> GetAllDepartments();
        Task<DepartmentModel> GetDepartmentById(int id);
        Task<IEnumerable<RoleModel>> GetAllRoles();
        Task<RoleModel> GetRoleById(int id);
        Task<IEnumerable<RoleModel>> GetRoleByDepartmentId(int departmentId);
        Task<IEnumerable<PositionModel>> GetPositionsByRoleId(int roleId);
        Task<IEnumerable<PositionModel>> GetPositions();
    }
}
