using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class ModuleHierarchy : Entity
    {
        public int ModuleHierarchyId { get; set; }
        public int ModuleId { get; set; }
        public int ParentModuleId { get; set; }
        public bool IsActive { get; set; }

        public virtual Module Module { get; set; }
        public virtual Module ParentModule { get; set; }
    }
}
