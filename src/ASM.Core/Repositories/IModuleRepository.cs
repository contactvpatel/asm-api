using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories.Base;

namespace ASM.Core.Repositories
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<IEnumerable<Module>> Get();
        Task<Module> Create(Module module);
        Task Update(Module module);
        Task<IEnumerable<Module>> GetParentModules(int moduleId);
        Task<bool> IsModuleExists(Module module);
    }
}