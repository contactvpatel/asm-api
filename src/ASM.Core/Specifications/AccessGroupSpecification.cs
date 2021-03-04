using System;
using System.Linq;
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
            AddInclude(x => x.AccessGroupModulePermissions.Where(y => y.IsDeleted == false));
        }
        public AccessGroupSpecification(Guid applicationId)
            : base(x => x.IsDeleted == false && x.ApplicationId == applicationId && x.IsActive==true )
        {
            AddInclude(x => x.AccessGroupModulePermissions.Where(y => y.IsDeleted == false));
            ApplyOrderBy(x => x.Name);
        }
        public AccessGroupSpecification(Guid applicationId, int departmentId)
            : base(x => x.IsDeleted == false && x.ApplicationId == applicationId && x.DepartmentId == departmentId && x.IsActive == true)
        {
            AddInclude(x => x.AccessGroupModulePermissions.Where(y => y.IsDeleted == false));
            ApplyOrderBy(x => x.Name);
        }
    }
}