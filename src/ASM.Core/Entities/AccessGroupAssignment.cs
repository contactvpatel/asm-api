using ASM.Core.Entities.Base;

namespace ASM.Core.Entities
{
    public partial class AccessGroupAssignment : Entity
    {
        public int AccessGroupAssignmentId { get; set; }
        public int AccessGroupId { get; set; }
        public int? RoleId { get; set; }
        public int? PositionId { get; set; }
        public int? PersonId { get; set; }

        public virtual AccessGroup AccessGroup { get; set; }
    }
}
