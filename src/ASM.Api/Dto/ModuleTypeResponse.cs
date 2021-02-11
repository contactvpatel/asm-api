using System;

namespace ASM.Api.Dto
{
    public class ModuleTypeResponse
    {
        public int ModuleTypeId { get; set; }
        public string Name { get; set; }
        public bool IsControlType { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
