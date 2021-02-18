using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Util.Models;
using IMisService = ASM.Core.Services.IMisService;

namespace ASM.Business.Services
{
    public class MisService : Interfaces.IMisService
    {
        private readonly IMisService _misService;

        public MisService(IMisService misService)
        {
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartments()
        {
            return await _misService.GetAllDepartments();
        }

        public async Task<DepartmentModel> GetDepartmentById(int id)
        {
            return await _misService.GetDepartmentById(id);
        }

        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            return await _misService.GetAllRoles();
        }

        public async Task<RoleModel> GetRoleById(int id)
        {
            return await _misService.GetRoleById(id);
        }

        public async Task<IEnumerable<RoleModel>> GetRoleByDepartmentId(int departmentId)
        {
            return await _misService.GetRoleByDepartmentId(departmentId);
        }

        public async Task<IEnumerable<PositionModel>> GetPositions(int roleId)
        {
            return await _misService.GetPositions(roleId);
        }
    }
}
