using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Util.Models;
using ISsoService = ASM.Core.Services.ISsoService;

namespace ASM.Business.Services
{
    public class SsoService : Interfaces.ISsoService
    {
        private readonly ISsoService _ssoService;
        private readonly Interfaces.IRedisCacheService _redisCacheService;

        public SsoService(ISsoService ssoService, Interfaces.IRedisCacheService redisCacheService)
        {
            _ssoService = ssoService ?? throw new ArgumentNullException(nameof(ssoService));
            _redisCacheService = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
        }

        public async Task<IEnumerable<ApplicationModel>> GetAllApplications()
        {
            const string cacheKey = "applications";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<ApplicationModel>>(cacheKey);
            if (cachedResponse != null)
                return cachedResponse;

            var applications = await _ssoService.GetAllApplications();

            await _redisCacheService.SetCacheData(cacheKey, applications, TimeSpan.FromSeconds(86400));

            return applications;
        }

        public async Task<ApplicationModel> GetApplicationById(Guid id)
        {
            const string cacheKey = "applications";

            var cachedResponse = await _redisCacheService.GetCachedData<IEnumerable<ApplicationModel>>(cacheKey);
            if (cachedResponse != null)
            {
                var applications = await _ssoService.GetAllApplications();
                return applications.FirstOrDefault(x => x.ApplicationId == id);
            }

            return await _ssoService.GetApplicationById(id);
        }

        public async Task<bool> ValidateToken(string token)
        {
            return await _ssoService.ValidateToken(token);
        }

        public async Task<bool> Logout(string token)
        {
            return await _ssoService.Logout(token);
        }
    }
}
