using System;
using System.Collections.Generic;
using ASM.Util.Models;

namespace ASM.Business.Interfaces
{
    public interface IApplicationSecurityService
    {
        IEnumerable<ApplicationSecurityModel> Get(Guid applicationId, int? roleId, int? positionId, int? personId);
    }
}
