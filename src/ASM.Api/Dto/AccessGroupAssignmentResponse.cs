namespace ASM.Api.Dto
{
    public class AccessGroupAssignmentResponse
    {
        public int AccessGroupAssignmentId { get; set; }
        public int AccessGroupId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? PersonId { get; set; }
        public string PersonName { get; set; }
    }
}
