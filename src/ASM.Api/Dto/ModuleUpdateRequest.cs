using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASM.Api.Dto
{
    public class ModuleUpdateRequest
    {
        [Required] 
        public int ModuleId { get; set; }
        
        [Required] 
        public string Name { get; set; }
        
        [Required] 
        public string Code { get; set; }

        [Required(ErrorMessage = "Module Type is required")]
        public int ModuleTypeId { get; set; }

        [Required(ErrorMessage = "Application is required")]
        public Guid ApplicationId { get; set; }

        public int? ParentModuleId { get; set; }

        public bool IsActive { get; set; }
        
        public int LastUpdatedBy { get; set; }
    }
}
