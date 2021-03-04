using System;
using ASM.Business.Models.Base;

namespace ASM.Business.Models
{
    public class ModuleModel : BaseModel
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ApplicationName { get; set; }
        public int ModuleTypeId { get; set; }
        public Guid ApplicationId { get; set; }
        public int? ParentModuleId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ModuleTypeModel ModuleType { get; set; }
        public virtual ModuleModel ParentModule { get; set; }
    }
}
