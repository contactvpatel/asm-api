using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ASM.Api.Attributes;

namespace ASM.Api.Dto
{
    public class AccessGroupCreateRequest
    {
        [Required] 
        public string Name { get; set; }


        public string Description { get; set; }

        [NotEmpty(ErrorMessage = "Application is required and shouldn't be empty")]
        public Guid ApplicationId { get; set; }

        public int? DepartmentId { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual ICollection<AccessGroupModulePermissionCreateRequest> AccessGroupModulePermissions { get; set; }
    }
}
