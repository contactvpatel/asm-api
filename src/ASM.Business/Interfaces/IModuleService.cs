using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Business.Models;

namespace ASM.Business.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleModel>> GetAll();
        Task<ModuleModel> GetById(int id);
        Task<IEnumerable<ModuleModel>> GetByApplicationId(Guid applicationId);
        Task<ModuleModel> Create(ModuleModel module);
        Task Update(ModuleModel module);
        Task Delete(int id, int userId);
        Task<bool> IsModuleExists(ModuleModel module);
    }
}