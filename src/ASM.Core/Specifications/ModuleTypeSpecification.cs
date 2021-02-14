using ASM.Core.Entities;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Specifications
{
    public class ModuleTypeSpecification : BaseSpecification<ModuleType>
    {
        public ModuleTypeSpecification() : base(b => b.IsActive == true && b.IsDeleted == false)
        {
            ApplyOrderBy(x => x.Name);
        }
    }
}
