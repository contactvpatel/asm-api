using System;
using System.ComponentModel.DataAnnotations;
using ASM.Api.Attributes;

namespace ASM.Api.Dto
{
    public class ModuleCreateRequest
    {
        [Required] 
        public string Name { get; set; }

        [Required] 
        public string Code { get; set; }

        [Required(ErrorMessage = "Module Type is required")]
        public int ModuleTypeId { get; set; }

        [NotEmpty(ErrorMessage = "Application is required and shouldn't be empty")]
        public Guid ApplicationId { get; set; }

        public int? ParentModuleId { get; set; }

        public bool IsActive { get; set; }

        [Required] 
        public int UserId { get; set; }
    }
}
