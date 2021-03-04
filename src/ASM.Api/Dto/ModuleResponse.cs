using System;

namespace ASM.Api.Dto
{
    public class ModuleResponse
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ModuleTypeId { get; set; }
        public Guid ApplicationId { get; set; }
        public int? ParentModuleId { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }
        public virtual ModuleTypeResponse ModuleType { get; set; }
        public virtual ModuleResponse ParentModule { get; set; }
    }
}
