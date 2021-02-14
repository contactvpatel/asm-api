using System;
using ASM.Business.Models.Base;

namespace ASM.Business.Models
{
    public class AccessGroupAssignmentModel : BaseModel
    {
        public int AccessGroupAssignmentId { get; set; }
        public int AccessGroupId { get; set; }
        public string Name { get; set; }
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? PersonId { get; set; }
        public string PersonName { get; set; }
    }
}
