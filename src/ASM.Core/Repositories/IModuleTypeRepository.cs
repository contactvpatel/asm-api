using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories.Base;

namespace ASM.Core.Repositories
{
    public interface IModuleTypeRepository : IRepository<ModuleType>
    {
        Task<IEnumerable<ModuleType>> GetAll();
    }
}
