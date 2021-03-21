using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Util.Models;

namespace ASM.Core.Services
{
    public interface ISsoService
    {
        Task<IEnumerable<ApplicationModel>> GetAllApplications();
        Task<ApplicationModel> GetApplicationById(Guid id);
        Task<bool> ValidateToken(string token);
        Task<bool> Logout(string token);
    }
}
