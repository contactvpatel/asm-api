using System;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Infrastructure.Data;
using ASM.Infrastructure.Repositories.Base;

namespace ASM.Infrastructure.Repositories
{
    public class ModuleHierarchyRepository : Repository<ModuleHierarchy>, IModuleHierarchyRepository
    {
        private readonly ASMContext _asmContext;

        public ModuleHierarchyRepository(ASMContext asmContext) : base(asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }
    }
}
