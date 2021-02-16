using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Core.Specifications;
using ASM.Infrastructure.Data;
using ASM.Infrastructure.Repositories.Base;

namespace ASM.Infrastructure.Repositories
{
    public class ModuleTypeRepository : Repository<ModuleType>, IModuleTypeRepository
    {
        private readonly ASMContext _asmContext;

        public ModuleTypeRepository(ASMContext asmContext) : base(asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }

        public async Task<IEnumerable<ModuleType>> GetAll()
        {
            var spec = new ModuleTypeSpecification();
            return await GetAsync(spec);
        }
    }
}
