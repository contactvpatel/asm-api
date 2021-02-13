using System.Collections.Generic;
using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class ModuleType : Entity
    {
        public ModuleType()
        {
            Modules = new HashSet<Module>();
        }

        public int ModuleTypeId { get; set; }
        public string Name { get; set; }
        public bool IsControlType { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Module> Modules { get; set; }
    }
}
