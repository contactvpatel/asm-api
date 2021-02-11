using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class AccessGroupModulePermission : Entity
    {
        public int AccessGroupModulePermissionId { get; set; }
        public int AccessGroupId { get; set; }
        public int ModuleId { get; set; }
        public int PermissionId { get; set; }

        public virtual AccessGroup AccessGroup { get; set; }
        public virtual Module Module { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
