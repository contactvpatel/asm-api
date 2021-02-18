using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Util.Models;
using ISsoService = ASM.Core.Services.ISsoService;

namespace ASM.Business.Services
{
    public class SsoService : Interfaces.ISsoService
    {
        private readonly ISsoService _ssoService;

        public SsoService(ISsoService ssoService)
        {
            _ssoService = ssoService ?? throw new ArgumentNullException(nameof(ssoService));
        }

        public async Task<IEnumerable<ApplicationModel>> GetAllApplications()
        {
            return await _ssoService.GetAllApplications();
        }

        public async Task<ApplicationModel> GetApplicationById(Guid id)
        {
            return await _ssoService.GetApplicationById(id);
        }
    }
}
