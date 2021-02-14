namespace ASM.Api.Dto
{
    public class AccessGroupModulePermissionResponse
    {
        public int AccessGroupModulePermissionId { get; set; }
        public int AccessGroupId { get; set; }
        public int ModuleId { get; set; }
        public bool HasViewAccess { get; set; }
        public bool HasCreateAccess { get; set; }
        public bool HasUpdateAccess { get; set; }
        public bool HasDeleteAccess { get; set; }
        public bool HasAccessRight { get; set; }
    }
}
