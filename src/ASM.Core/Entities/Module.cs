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
            ModuleHierarchyModules = new HashSet<ModuleHierarchy>();
            ModuleHierarchyParentModules = new HashSet<ModuleHierarchy>();
        }

        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ModuleTypeId { get; set; }
        public Guid ApplicationId { get; set; }
        public bool IsActive { get; set; }

        public virtual ModuleType ModuleType { get; set; }
        public virtual ICollection<AccessGroupModulePermission> AccessGroupModulePermissions { get; set; }
        public virtual ICollection<ModuleHierarchy> ModuleHierarchyModules { get; set; }
        public virtual ICollection<ModuleHierarchy> ModuleHierarchyParentModules { get; set; }
    }
}
