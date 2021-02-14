using System;

namespace ASM.Api.Dto
{
    public class AccessGroupAssignmentResponse
    {
        public int AccessGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int AccessGroupAssignmentId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? PersonId { get; set; }
        public string PersonName { get; set; }
    }
}
