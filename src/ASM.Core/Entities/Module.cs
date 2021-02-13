using System;
using System.Collections.Generic;
using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class Module : Entity
    {
        public Module()
        {
            AccessGroupModulePermissions = new HashSet<AccessGroupModulePermission>();
            InverseParentModule = new HashSet<Module>();
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ModuleTypeId { get; set; }
        public Guid ApplicationId { get; set; }
        public int? ParentModuleId { get; set; }
        public bool? IsActive { get; set; }

        public virtual ModuleType ModuleType { get; set; }
        public virtual Module ParentModule { get; set; }
        public virtual ICollection<AccessGroupModulePermission> AccessGroupModulePermissions { get; set; }
        public virtual ICollection<Module> InverseParentModule { get; set; }
    }
}
