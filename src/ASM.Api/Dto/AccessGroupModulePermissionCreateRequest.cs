using System.ComponentModel.DataAnnotations;

namespace ASM.Api.Dto
{
    public class AccessGroupModulePermissionCreateRequest
    {
        [Required]
        public int ModuleId { get; set; }

        public bool HasViewAccess { get; set; }

        public bool HasCreateAccess { get; set; }

        public bool HasUpdateAccess { get; set; }

        public bool HasDeleteAccess { get; set; }

        public bool HasAccessRight { get; set; }
    }
}
