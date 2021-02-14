namespace ASM.Util.Models
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int DivisionGeoLevelId { get; set; }
        public string DivisionGeoLevelName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Wing { get; set; }
        public bool? IsApplicationDepartment { get; set; }
        public bool? IsAdministrationDepartment { get; set; }
        public bool? IsServicesDepartment { get; set; }
    }
}
