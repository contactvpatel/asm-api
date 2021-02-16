using System;
using System.Collections.Generic;
using ASM.Business.Interfaces;
using ASM.Core.Repositories;
using ASM.Util.Models;
using Microsoft.Extensions.Logging;

namespace ASM.Business.Services
{
    public class ApplicationSecurityService : IApplicationSecurityService
    {
        private readonly IApplicationSecurityRepository _applicationSecurityRepository;
        private readonly ILogger<AccessGroupService> _logger;

        public ApplicationSecurityService(IApplicationSecurityRepository applicationSecurityRepository,
            ILogger<AccessGroupService> logger)
        {
            _applicationSecurityRepository = applicationSecurityRepository ??
                                             throw new ArgumentNullException(nameof(applicationSecurityRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<ApplicationSecurityModel> Get(Guid applicationId, int? roleId, int? positionId,
            int? personId)
        {
            return _applicationSecurityRepository.Get(applicationId, roleId, positionId, personId);
        }
    }
}
