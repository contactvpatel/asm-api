using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories.Base;

namespace ASM.Core.Repositories
{
    public interface IModuleRepository : IRepository<Module>
    {
        Task<IEnumerable<Module>> GetAll();
        Task<Module> GetById(int id);
        Task<IEnumerable<Module>> GetByApplicationId(Guid applicationId);
        Task<bool> IsModuleExists(Module module);
    }
}