using System;
using ASM.Core.Entities;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Specifications
{
    public class AccessGroupSpecification : BaseSpecification<AccessGroup>
    {
        public AccessGroupSpecification()
            : base(b => b.IsDeleted == false)
        {
            AddInclude(p => p.AccessGroupModulePermissions);
            ApplyOrderBy(x => x.ApplicationId);
            ApplyOrderBy(x => x.DepartmentId);
            ApplyOrderBy(x => x.Name);
        }

        public AccessGroupSpecification(int id)
            : base(b => b.IsDeleted == false && b.AccessGroupId == id)
        {
            AddInclude(p => p.AccessGroupModulePermissions);
        }

        public AccessGroupSpecification(Guid applicationId, int departmentId)
            : base(b => b.IsDeleted == false && b.ApplicationId == applicationId && b.DepartmentId == departmentId)
        {
            AddInclude(p => p.AccessGroupModulePermissions);
            ApplyOrderBy(x => x.Name);
        }
    }
}