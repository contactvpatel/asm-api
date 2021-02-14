namespace ASM.Util.Models
{
    public class PositionModel
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionShortName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int RolePositionEntityId { get; set; }
        public string RolePositionEntityName { get; set; }
        public int PersonId { get; set; }
        public string Wing { get; set; }
        public bool? IsApplicationDepartment { get; set; }
        public bool? IsAdministrationDepartment { get; set; }
        public bool? IsServicesDepartment { get; set; }
    }
}
