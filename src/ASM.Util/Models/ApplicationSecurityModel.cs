namespace ASM.Util.Models
{
    public class ApplicationSecurityModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public bool IsControlType { get; set; }
        public bool HasViewAccess { get; set; }
        public bool HasCreateAccess { get; set; }
        public bool HasUpdateAccess { get; set; }
        public bool HasDeleteAccess { get; set; }
        public bool HasAccessRight { get; set; }
    }
}