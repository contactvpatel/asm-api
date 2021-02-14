using System.Collections.Generic;
using ASM.Business.Models.Base;

namespace ASM.Business.Models
{
    public class AccessGroupAssignmentModel : BaseModel
    {
        public int AccessGroupAssignmentId { get; set; }
        public int AccessGroupId { get; set; }
        public int? RoleId { get; set; }
        public int? PositionId { get; set; }
        public int? PersonId { get; set; }
    }
}
