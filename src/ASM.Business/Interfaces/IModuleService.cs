using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Business.Models;

namespace ASM.Business.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleModel>> Get();
        Task<ModuleModel> GetById(int id);
        Task<IEnumerable<ModuleModel>> GetParentModules(int moduleId);
        Task<ModuleModel> Create(ModuleModel module);
        Task Update(ModuleModel module);
        Task Delete(ModuleModel module);
        Task<bool> IsModuleExists(ModuleModel module);
    }
}