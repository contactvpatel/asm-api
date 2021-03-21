using System;
using System.Collections.Generic;
using ASM.Api.Dto;
using ASM.Business.Interfaces;
using ASM.Util.Logging;
using ASM.Util.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASM.Api.Controllers
{
    /// <summary>
    /// Application Security Controller. 
    /// Contain Application Security API. 
    /// </summary>
    [Route("api/v{version:apiVersion}/application-security")]
    [ApiController]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class ApplicationSecurityController : ControllerBase
    {
        private readonly IApplicationSecurityService _applicationSecurityService;
        private readonly ILogger<ApplicationSecurityController> _logger;

        public ApplicationSecurityController(IApplicationSecurityService applicationSecurityService,
            ILogger<ApplicationSecurityController> logger)
        {
            _applicationSecurityService = applicationSecurityService ??
                                          throw new ArgumentNullException(nameof(applicationSecurityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Return Application Security.
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="roleId">Role Id</param>
        /// <param name="positionId">Position Id</param>
        /// <param name="personId">Person Id</param>
        [HttpGet(Name = "GetApplicationSecurity")]
        public ActionResult<ApplicationSecurityModel> Get(Guid applicationId, int? roleId, int? positionId,
            int? personId)
        {
            _logger.LogInformationExtension(
                $"Get Application Security by Application Id: {applicationId}, Role Id: {roleId}, Position Id: {positionId}, Person Id: {personId}");

            var applicationSecurityModels =
                _applicationSecurityService.Get(applicationId, roleId, positionId, personId);

            if (applicationSecurityModels == null)
            {
                var message =
                    $"No Application Security by Application Id: {applicationId}, Role Id: {roleId}, Position Id: {positionId}, Person Id: {personId}";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<ModuleResponse>(false, message));
            }

            return Ok(new Models.Response<IEnumerable<ApplicationSecurityModel>>(applicationSecurityModels));
        }
    }
}
