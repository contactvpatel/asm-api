using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Infrastructure.Data;
using ASM.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ASM.Infrastructure.Repositories
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        private readonly ASMContext _asmContext;

        public ModuleRepository(ASMContext asmContext) : base(asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }

        public async Task<IEnumerable<Module>> Get()
        {
            return await _asmContext.Modules
                .Include(x => x.ModuleType)
                .Include(x => x.ModuleHierarchyModules)
                .Where(x => x.IsActive && x.IsDeleted == false)
                .OrderBy(x => x.ApplicationId)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Module> Create(Module module)
        {
            await using var transaction = await _asmContext.Database.BeginTransactionAsync();

            await _asmContext.Modules.AddAsync(module);
            await _asmContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return module;
        }

        public async Task Update(Module module)
        {
            await using var transaction = await _asmContext.Database.BeginTransactionAsync();

            var existingModule = await _asmContext.Modules.FirstOrDefaultAsync(x => x.ModuleId == module.ModuleId);

            var existingModuleHierarchies = _asmContext.ModuleHierarchies.Where(x => x.ModuleId == module.ModuleId);
            foreach (var currentHierarchy in existingModuleHierarchies)
            {
                currentHierarchy.IsDeleted = true;
                currentHierarchy.LastUpdated = DateTime.Now;
            }

            _asmContext.ModuleHierarchies.RemoveRange(existingModuleHierarchies);

            module.CreatedBy = existingModule.CreatedBy;
            module.Created = existingModule.Created;
            module.LastUpdated = DateTime.Now;
            
            _asmContext.Modules.Update(module);

            await _asmContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<IEnumerable<Module>> GetParentModules(int moduleId)
        {
            return await _asmContext.Modules
                .Where(p => p.ModuleId != moduleId)
                .ToListAsync();
        }

        public async Task<bool> IsModuleExists(Module module)
        {
            Module existModule;
            if (module.ModuleId == 0)
            {
                existModule = await _asmContext.Modules.FirstOrDefaultAsync(x =>
                    x.ApplicationId == module.ApplicationId && x.Code == module.Code &&
                    x.Name == module.Name && x.ModuleTypeId == module.ModuleTypeId);
            }
            else
            {
                existModule = await _asmContext.Modules.FirstOrDefaultAsync(x =>
                    x.ModuleId != module.ModuleId && x.ApplicationId == module.ApplicationId && x.Code == module.Code &&
                    x.Name == module.Name && x.ModuleTypeId == module.ModuleTypeId);
            }

            return existModule != null;
        }
    }
}
