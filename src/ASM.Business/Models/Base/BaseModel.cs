using System;

namespace ASM.Business.Models.Base
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public int LastUpdatedBy { get; set; }
    }
}
