using System.Collections.Generic;

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

    public class MisResponse<T>
    {
        public bool Status { get; set; }
        public List<T> Data { get; set; }
        public List<Messages> Messages { get; set; }
    }

    public class Messages
    {
        public string ErrorMessage { get; set; }
        public string FieldName { get; set; }
        public string ErrorCode { get; set; }
    }
}
