using System.Collections.Generic;
using ASM.Util.Models;

namespace ASM.Core.Models
{
    public class MisApiModel
    {
        public string Url { get; set; }
        public MisEndpoint Endpoint { get; set; }
    }

    public class MisEndpoint
    {
        public string Department { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }
        public string PersonPosition { get; set; }
    }

    public class DepartmentData
    {
        public IEnumerable<DepartmentModel> Data { get; set; }
    }

    public class PositionData
    {
        public IEnumerable<PositionModel> Data { get; set; }
    }
    public class RoleData
    {
        public IEnumerable<RoleModel> Data { get; set; }
    }
    public class PersonPositionData
    {
        public IEnumerable<PositionModel> Data { get; set; }
    }
}
