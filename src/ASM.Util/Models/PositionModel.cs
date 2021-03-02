namespace ASM.Util.Models
{
    public class PositionModel
    {
        //public int RolePositionId { get; set; }
        //public string RolePositionName { get; set; }
        //public string RolePositionShortName { get; set; }
        //public int RoleId { get; set; }
        //public string RoleName { get; set; }
        //public int DepartmentId { get; set; }
        //public string DepartmentName { get; set; }
        //public int RolePositionEntityId { get; set; }
        //public string RolePositionEntityName { get; set; }
        //public int PersonId { get; set; }
        //public string Wing { get; set; }
        //public bool? IsApplicationDepartment { get; set; }
        //public bool? IsAdministrationDepartment { get; set; }
        //public bool? IsServicesDepartment { get; set; }


        public int? positionId { get; set; }
        public int? departmentId { get; set; }
        public string departmentName { get; set; }
        public int? roleId { get; set; }
        public string roleName { get; set; }
        public int? divisionGeoLevelEntityTypeId { get; set; }
        public string divisionGeoLevelType { get; set; }
        public int? positionEntityId { get; set; }
      public string positionEntityName { get; set; }
      public int? occurance { get; set; }
      public string positionName { get; set; }
      public string positionShortName { get; set; }
    public  string positionWithRoleName { get; set; }
        public int? personId { get; set; }
      
      public bool? isSatsangActivityDepartment { get; set; }
      public string wing { get; set; }
      public bool? isSampark { get; set; }
        public bool? isApplicationDepartment { get; set; }
        public bool? isAdministrationDepartment { get; set; }
        public bool? isServicesDepartment { get; set; }
        public bool? rolePositionEntity { get; set; }
    }
}
