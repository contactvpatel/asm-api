using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Util.Models;
using IMisService = ASM.Core.Services.IMisService;

namespace ASM.Business.Services
{
    public class MisService : Interfaces.IMisService
    {
        private readonly IMisService _misService;
        private readonly Interfaces.IRedisCacheService _redisCacheService;

        public MisService(IMisService misService, Interfaces.IRedisCacheService redisCacheService)
        {
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
            _redisCacheService = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllDepartments()
        {
            const string cacheKey = "departments";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<DepartmentModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse;

            var departments = await _misService.GetAllDepartments();

            await _redisCacheService.SetCacheData(cacheKey, departments, TimeSpan.FromSeconds(86400));

            return departments;
        }

        public async Task<DepartmentModel> GetDepartmentById(int id)
        {
            const string cacheKey = "departments";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<DepartmentModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse.FirstOrDefault(x => x.DepartmentId == id);

            return await _misService.GetDepartmentById(id);
        }

        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            const string cacheKey = "roles";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<RoleModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse;

            var roles = await _misService.GetAllRoles();

            await _redisCacheService.SetCacheData(cacheKey, roles, TimeSpan.FromSeconds(86400));

            return roles;
        }

        public async Task<RoleModel> GetRoleById(int id)
        {
            const string cacheKey = "roles";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<RoleModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse.FirstOrDefault(x => x.RoleId == id);

            return await _misService.GetRoleById(id);
        }

        public async Task<IEnumerable<RoleModel>> GetRoleByDepartmentId(int departmentId)
        {
            const string cacheKey = "roles";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<RoleModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse.Where(x => x.DepartmentId == departmentId);

            return await _misService.GetRoleByDepartmentId(departmentId);
        }

        public async Task<IEnumerable<PositionModel>> GetPositions(int roleId)
        {
            return await _misService.GetPositionsByRoleId(roleId);
        }
    }
}
