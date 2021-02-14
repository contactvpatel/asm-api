namespace ASM.Api.Dto
{
    public class AccessGroupAssignmentRequest
    {
        public int AccessGroupId { get; set; }
        public int? RoleId { get; set; }
        public int? PositionId { get; set; }
        public int? PersonId { get; set; }
        public int UserId { get; set; }
    }
}
