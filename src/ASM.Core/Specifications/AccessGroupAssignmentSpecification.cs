using ASM.Core.Entities;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Specifications
{
    public class AccessGroupAssignmentSpecification : BaseSpecification<AccessGroupAssignment>
    {
        public AccessGroupAssignmentSpecification()
            : base(x => x.IsDeleted == false)
        {
            AddInclude(x => x.AccessGroup);
            ApplyOrderBy(x => x.AccessGroup.Name);
            ApplyOrderBy(x => x.AccessGroup.ApplicationId);
            ApplyOrderBy(x => x.AccessGroup.DepartmentId);
        }
    }
}