using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Core.Specifications;
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

        public async Task<IEnumerable<Module>> GetAll()
        {
            var spec = new ModuleSpecification();
            return await GetAsync(spec);
        }

        public async Task<Module> GetById(int id)
        {
            var spec = new ModuleSpecification(id);
            return (await GetAsync(spec)).FirstOrDefault();
        }

        public async Task<IEnumerable<Module>> GetByApplicationId(Guid applicationId)
        {
            var spec = new ModuleSpecification(applicationId);
            return await GetAsync(spec);
        }

        public async Task Delete(int id, int userId)
        {
            await using var transaction = await _asmContext.Database.BeginTransactionAsync();

            var existingModule = await _asmContext.Modules.FirstOrDefaultAsync(x => x.ModuleId == id);
            if (existingModule == null)
                throw new ApplicationException($"Not able to find Module with id: {id}");

            existingModule.IsDeleted = true;
            existingModule.LastUpdated = DateTime.Now;
            existingModule.LastUpdatedBy = userId;

            var accessGroupModulePermissions =
                _asmContext.AccessGroupModulePermissions.Where(x => x.IsDeleted == false && x.ModuleId == id);
            foreach (var currentAccessGroupModulePermission in accessGroupModulePermissions)
            {
                currentAccessGroupModulePermission.IsDeleted = true;
                currentAccessGroupModulePermission.LastUpdated = DateTime.Now;
                currentAccessGroupModulePermission.LastUpdatedBy = userId;
            }

            _asmContext.Update(existingModule);

            await _asmContext.SaveChangesAsync();

            await transaction.CommitAsync();
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
