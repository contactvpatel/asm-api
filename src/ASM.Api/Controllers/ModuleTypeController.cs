using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Api.Dto;
using ASM.Business.Interfaces;
using ASM.Util.Logging;
using ASM.Util.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASM.Api.Controllers
{
    /// <summary>
    /// Module Type Controller. 
    /// Contain Module Type API
    /// </summary>
    [Route("api/v{version:apiVersion}/module-types")]
    [ApiController]
    [ApiVersion("1.0")]

    public class ModuleTypeController : ControllerBase
    {
        private readonly IModuleTypeService _moduleTypeService;
        private readonly ILogger<ModuleTypeController> _logger;
        private readonly IMapper _mapper;

        public ModuleTypeController(IModuleTypeService moduleTypeService, ILogger<ModuleTypeController> logger,
            IMapper mapper)
        {
            _moduleTypeService = moduleTypeService ?? throw new ArgumentNullException(nameof(moduleTypeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return Model Type List.
        /// </summary>                
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleTypeResponse>>> GetAll()
        {
            _logger.LogInformationExtension("Get All Module Types");
            var moduleTypes = await _moduleTypeService.GetAll();
            string message;

            if (moduleTypes == null)
            {
                message = "No module types found";
                _logger.LogErrorExtension(message, null);
                return NotFound(new Models.Response<ModuleTypeResponse>(false, message));
            }

            message = $"Found {moduleTypes.Count()} module types";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<ModuleTypeResponse>>(
                _mapper.Map<IEnumerable<ModuleTypeResponse>>(moduleTypes), message));
        }
    }
}
