namespace ASM.Core.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentShortName { get; set; }
        public int DivisionId { get; set; }
        public string Wing { get; set; }
        public bool? IsSatsangActivityDepartment { get; set; }
        public bool? IsApplicationDepartment { get; set; }
        public bool? IsAdministrationDepartment { get; set; }
        public bool? IsServicesDepartment { get; set; }
    }
}
