using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Core.Specifications;
using ASM.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ASM.Infrastructure.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        private readonly Data.ASMContext _asmContext;

        public ModuleRepository(Data.ASMContext asmContext) : base(asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }

        public async Task<IEnumerable<Module>> GetAll()
        {
            var spec = new ModuleSpecification();
            return (await GetAsync(spec)).OrderBy(x => x.ApplicationId).ThenBy(x => x.Name).ToList();
        }

        public async Task<Module> GetById(int id)
        {
            var spec = new ModuleSpecification(id);
            return (await GetAsync(spec)).FirstOrDefault();
        }

        public async Task<IEnumerable<Module>> GetByApplicationId(Guid applicationId)
        {
            var spec = new ModuleSpecification(applicationId);
            return (await GetAsync(spec)).OrderBy(x => x.Name).ToList();
        }

        public async Task<bool> IsModuleExists(Module module)
        {
            Module existModule;
            if (module.ModuleId == 0)
            {
                existModule = await _asmContext.Modules.AsNoTracking().FirstOrDefaultAsync(x =>
                    x.ApplicationId == module.ApplicationId && x.Code == module.Code &&
                    x.Name == module.Name && x.ModuleTypeId == module.ModuleTypeId && x.IsDeleted == false);
            }
            else
            {
                existModule = await _asmContext.Modules.AsNoTracking().FirstOrDefaultAsync(x =>
                    x.ModuleId != module.ModuleId && x.ApplicationId == module.ApplicationId && x.Code == module.Code &&
                    x.Name == module.Name && x.ModuleTypeId == module.ModuleTypeId && x.IsDeleted == false);
            }

            return existModule != null;
        }
    }
}
