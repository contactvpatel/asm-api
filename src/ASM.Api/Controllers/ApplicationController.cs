using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Business.Interfaces;
using ASM.Util.Logging;
using ASM.Util.Models;
using Microsoft.Extensions.Logging;

namespace ASM.Api.Controllers
{
    [Route("api/v{version:apiVersion}/applications")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ApplicationController : ControllerBase
    {
        private readonly ISsoService _ssoService;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(ISsoService ssoService, ILogger<ApplicationController> logger)
        {
            _ssoService = ssoService ?? throw new ArgumentNullException(nameof(ssoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Return All Application List.
        /// </summary>                
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationModel>>> GetAll()
        {
            _logger.LogInformationExtension("Get All Applications");
            var applications = await _ssoService.GetAllApplications();
            string message;

            if (applications == null)
            {
                message = "No applications found";
                _logger.LogErrorExtension(message, null);
                return NotFound(new Models.Response<ApplicationModel>(false, message));
            }

            message = $"Found {applications.Count()} applications";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<ApplicationModel>>(applications, message));
        }

        /// <summary>
        /// Return Application By Id.
        /// </summary>
        /// <param name="id">Application Id.</param>
        [HttpGet("{id}", Name = "GetApplicationById")]
        public async Task<ActionResult<ApplicationModel>> GetByDepartmentId(Guid id)
        {
            _logger.LogInformationExtension($"Get Application By Id: {id}");

            var application = await _ssoService.GetApplicationById(id);

            if (application == null)
            {
                var message = $"No application found by id: {id}";
                _logger.LogErrorExtension(message, null);
                return NotFound(new Models.Response<DepartmentModel>(false, message));
            }

            return Ok(new Models.Response<ApplicationModel>(application));
        }
    }
}
