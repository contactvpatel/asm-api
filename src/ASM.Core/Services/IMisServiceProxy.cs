using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;

namespace ASM.Core.Services
{
    public interface IMisServiceProxy
    {
        Task<IEnumerable<Department>> GetDepartment();
        Task<Department> GetDepartmentById(int id);
        Task<IEnumerable<Role>> GetRoleByDepartmentId(int roleId);
        Task<IEnumerable<Position>> GetPosition(int roleId, int departmentId);
        Task<IEnumerable<Position>> GetPerson(int personId);
    }
}
