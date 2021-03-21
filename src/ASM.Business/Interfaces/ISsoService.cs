using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Util.Models;

namespace ASM.Business.Interfaces
{
    public interface ISsoService
    {
        Task<IEnumerable<ApplicationModel>> GetAllApplications();
        Task<ApplicationModel> GetApplicationById(Guid id);
        Task<bool> ValidateToken(string token);
        Task<bool> Logout(string token);
    }
}
