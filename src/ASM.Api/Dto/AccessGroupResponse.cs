using System;
using System.Collections.Generic;

namespace ASM.Api.Dto
{
    public class AccessGroupResponse
    {
        public int AccessGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdatedBy { get; set; }

        public virtual ICollection<AccessGroupModulePermissionUpdateRequest> AccessGroupModulePermissions { get; set; }
    }
}
