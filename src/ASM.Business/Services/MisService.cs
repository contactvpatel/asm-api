using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Business.Mapper;
using ASM.Util.Models;
using Microsoft.Extensions.Logging;
using IMisService = ASM.Core.Services.IMisService;

namespace ASM.Business.Services
{
    public class MisService : Interfaces.IMisService
    {
        private readonly IMisService _misService;
        private readonly ILogger<MisService> _logger;

        public MisService(IMisService misService, ILogger<MisService> logger)
        {
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartments()
        {
            return ObjectMapper.Mapper.Map<IEnumerable<DepartmentModel>>(await _misService.GetAllDepartments());
        }

        public async Task<DepartmentModel> GetDepartmentById(int id)
        {
            return ObjectMapper.Mapper.Map<DepartmentModel>(await _misService.GetDepartmentById(id));
        }

        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            return ObjectMapper.Mapper.Map<IEnumerable<RoleModel>>(await _misService.GetAllRoles());
        }

        public async Task<IEnumerable<RoleModel>> GetRoleByDepartmentId(int departmentId)
        {
            return ObjectMapper.Mapper.Map<IEnumerable<RoleModel>>(
                await _misService.GetRoleByDepartmentId(departmentId));
        }

        public async Task<IEnumerable<PositionModel>> GetPositions(int roleId)
        {
            return ObjectMapper.Mapper.Map<IEnumerable<PositionModel>>(
                await _misService.GetPositions(roleId));
        }
    }
}
