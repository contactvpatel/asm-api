using System.Collections.Generic;
using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class Permission : Entity
    {
        public Permission()
        {
            AccessGroupModulePermissions = new HashSet<AccessGroupModulePermission>();
        }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public bool IsControlType { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<AccessGroupModulePermission> AccessGroupModulePermissions { get; set; }
    }
}
