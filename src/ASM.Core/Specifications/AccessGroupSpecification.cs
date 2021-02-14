using System;
using ASM.Core.Entities;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Specifications
{
    public class AccessGroupSpecification : BaseSpecification<AccessGroup>
    {
        public AccessGroupSpecification()
            : base(x => x.IsDeleted == false)
        {
            ApplyOrderBy(x => x.ApplicationId);
            ApplyOrderBy(x => x.DepartmentId);
            ApplyOrderBy(x => x.Name);
        }

        public AccessGroupSpecification(int id)
            : base(x => x.IsDeleted == false && x.AccessGroupId == id)
        {
            AddInclude(x => x.AccessGroupModulePermissions);
        }

        public AccessGroupSpecification(Guid applicationId, int departmentId)
            : base(x => x.IsDeleted == false && x.ApplicationId == applicationId && x.DepartmentId == departmentId)
        {
            AddInclude(x => x.AccessGroupModulePermissions);
            ApplyOrderBy(x => x.Name);
        }
    }
}