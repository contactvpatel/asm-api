﻿using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class AccessGroupModulePermission : Entity
    {
        public int AccessGroupModulePermissionId { get; set; }
        public int AccessGroupId { get; set; }
        public int ModuleId { get; set; }
        public bool HasViewAccess { get; set; }
        public bool HasCreateAccess { get; set; }
        public bool HasUpdateAccess { get; set; }
        public bool HasDeleteAccess { get; set; }
        public bool HasAccessRight { get; set; }

        public virtual AccessGroup AccessGroup { get; set; }
        public virtual Module Module { get; set; }
    }
}
