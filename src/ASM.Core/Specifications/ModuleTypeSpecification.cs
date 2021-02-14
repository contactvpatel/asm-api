using ASM.Core.Entities;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Specifications
{
    public class ModuleTypeSpecification : BaseSpecification<ModuleType>
    {
        public ModuleTypeSpecification() : base(x => x.IsActive == true && x.IsDeleted == false)
        {
            ApplyOrderBy(x => x.Name);
        }
    }
}
