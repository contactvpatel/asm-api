using System;
using System.Collections.Generic;
using ASM.Util.Models;

namespace ASM.Core.Repositories
{
    public interface IApplicationSecurityRepository
    {
        IEnumerable<ApplicationSecurityModel> Get(Guid applicationId, int? roleId, int? positionId, int? personId);
    }
}
