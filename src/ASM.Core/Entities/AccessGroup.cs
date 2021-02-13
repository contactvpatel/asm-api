using System;
using System.Collections.Generic;
using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class AccessGroup : Entity
    {
        public AccessGroup()
        {
            AccessGroupAssignments = new HashSet<AccessGroupAssignment>();
            AccessGroupModulePermissions = new HashSet<AccessGroupModulePermission>();
        }

        public int AccessGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ApplicationId { get; set; }
        public int? DepartmentId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AccessGroupAssignment> AccessGroupAssignments { get; set; }
        public virtual ICollection<AccessGroupModulePermission> AccessGroupModulePermissions { get; set; }
    }
}
