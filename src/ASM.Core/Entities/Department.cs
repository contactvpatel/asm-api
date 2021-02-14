using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public class Department : Entity
    {
        public int DepartmentId { get; set; }
        public int DivisionId { get; set; }
        public string DepartmentName { get; set; }
    }
}
