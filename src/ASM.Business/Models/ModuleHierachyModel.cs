using ASM.Business.Models.Base;

namespace ASM.Business.Models
{
    public class ModuleHierachyModel : BaseModel
    {
        public int ModuleHierarchyId { get; set; }
        public int ModuleId { get; set; }
        public int ParentModuleId { get; set; }
    }
}