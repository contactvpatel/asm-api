using System.Collections.Generic;
using ASM.Core.Entities;

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
        public IEnumerable<Department> Data { get; set; }
    }

    public class PositionData
    {
        public IEnumerable<Position> Data { get; set; }
    }
    public class RoleData
    {
        public IEnumerable<Role> Data { get; set; }
    }
    public class PersonPositionData
    {
        public IEnumerable<Position> Data { get; set; }
    }
}
