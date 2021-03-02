using System;
using System.Collections.Generic;
using ASM.Business.Models.Base;

namespace ASM.Business.Models
{
    public class AccessGroupModel : BaseModel
    {
        public AccessGroupModel()
        {
            AccessGroupAssignments = new HashSet<AccessGroupAssignmentModel>();
            AccessGroupModulePermissions = new HashSet<AccessGroupModulePermissionModel>();
        }

        public int AccessGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ApplicationId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ApplicationName { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<AccessGroupAssignmentModel> AccessGroupAssignments { get; set; }
        public virtual ICollection<AccessGroupModulePermissionModel> AccessGroupModulePermissions { get; set; }
    }
}
