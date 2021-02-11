using ASM.Business.Models.Base;

namespace ASM.Business.Models
{
    public class ModuleTypeModel : BaseModel
    {
        public int ModuleTypeId { get; set; }
        public string Name { get; set; }
        public bool IsControlType { get; set; }
        public bool IsActive { get; set; }
    }
}
