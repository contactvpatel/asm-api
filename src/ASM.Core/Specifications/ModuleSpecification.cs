using System;
using ASM.Core.Entities;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Specifications
{
    public class ModuleSpecification : BaseSpecification<Module>
    {
        public ModuleSpecification() : base(x => x.IsDeleted == false)
        {
            AddInclude(x => x.ModuleType);
            AddInclude(x => x.ParentModule);
            ApplyOrderBy(x => x.ApplicationId);
            ApplyOrderBy(x => x.Name);
        }

        public ModuleSpecification(int id) : base(x => x.ModuleId == id)
        {
            AddInclude(x => x.ModuleType);
            AddInclude(x => x.ParentModule);
        }

        public ModuleSpecification(Guid applicationId) : base(x =>
            x.ApplicationId == applicationId && x.IsDeleted == false)
        {
            AddInclude(x => x.ModuleType);
            AddInclude(x => x.ParentModule);
            ApplyOrderBy(x => x.Name);
        }
    }
}
