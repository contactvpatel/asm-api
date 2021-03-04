namespace ASM.Util.Models
{
    public class PositionModel
    {
        public int PositionId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? DivisionGeoLevelEntityTypeId { get; set; }
        public string DivisionGeoLevelType { get; set; }
        public int? PositionEntityId { get; set; }
        public string PositionEntityName { get; set; }
        public int? Occurance { get; set; }
        public string PositionName { get; set; }
        public string PositionShortName { get; set; }
        public string PositionWithRoleName { get; set; }
        public int? PersonId { get; set; }
        public string AssignToPosition { get; set; }
        public bool IsSatsangActivityDepartment { get; set; }
        public string Wing { get; set; }
        public bool IsSampark { get; set; }
        public bool IsApplicationDepartment { get; set; }
        public bool IsAdministrationDepartment { get; set; }
        public string IsServicesDepartment { get; set; }
        public string RolePositionEntity { get; set; }
    }
}
