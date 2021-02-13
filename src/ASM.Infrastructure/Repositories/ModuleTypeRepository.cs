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
    public class ModuleTypeRepository : Repository<ModuleType>, IModuleTypeRepository
    {
        private readonly Data.ASMContext _asmContext;

        public ModuleTypeRepository(Data.ASMContext asmContext) : base(asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }

        public async Task<IEnumerable<ModuleType>> Get()
        {
            return await _asmContext.ModuleTypes
                .Where(x => x.IsActive == true && x.IsDeleted == false)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
