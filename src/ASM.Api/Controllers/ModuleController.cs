using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Api.Dto;
using ASM.Business.Interfaces;
using ASM.Business.Models;
using ASM.Util.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASM.Api.Controllers
{
    /// <summary>
    /// Module Controller. 
    /// Contain Module API. 
    /// </summary>
    [Route("api/v{version:apiVersion}/modules")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly ILogger<ModuleController> _logger;
        private readonly IMapper _mapper;

        public ModuleController(IModuleService moduleService, ILogger<ModuleController> logger, IMapper mapper)
        {
            _moduleService = moduleService ?? throw new ArgumentNullException(nameof(moduleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return Module List.
        /// </summary>        
        [HttpGet(Name = "GetAllModules")]
        public async Task<ActionResult<IEnumerable<ModuleResponse>>> Get()
        {
            _logger.LogInformationExtension("Get Modules");
            var modules = await _moduleService.GetAll();
            if (modules == null)
            {
                var message = "No modules found";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<ModuleResponse>(false, message));
            }

            _logger.LogInformationExtension($"Found {modules.Count()} modules");
            return Ok(new Models.Response<IEnumerable<ModuleResponse>>(
                _mapper.Map<IEnumerable<ModuleResponse>>(modules)));
        }

        /// <summary>
        /// Return Module By Id.
        /// </summary>
        /// <param name="id">ModuleId.</param>
        [HttpGet("{id}", Name = "GetModuleById")]
        public async Task<ActionResult<ModuleResponse>> GetById(int id)
        {
            _logger.LogInformationExtension($"Get Module By Id: {id}");
            var module = await _moduleService.GetById(id);
            if (module == null)
            {
                var message = $"No module found with id {id}";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<ModuleResponse>(false, message));
            }

            return Ok(new Models.Response<ModuleResponse>(_mapper.Map<ModuleResponse>(module)));
        }

        [HttpGet("applications/{id}", Name = "GetModuleByApplicationId")]
        public async Task<ActionResult<IEnumerable<ModuleResponse>>> Get(Guid id)
        {
            _logger.LogInformationExtension($"Get Modules by Application Id: {id}");
            var modules = await _moduleService.GetByApplicationId(id);
            if (modules == null)
            {
                var message = $"No modules found with application id {id}";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<ModuleResponse>(false, message));
            }

            _logger.LogInformationExtension($"Found {modules.Count()} modules by Application Id: {id}");
            return Ok(new Models.Response<IEnumerable<ModuleResponse>>(
                _mapper.Map<IEnumerable<ModuleResponse>>(modules)));
        }

        /// <summary>
        /// Create Module. 
        /// </summary>
        /// <param name="moduleCreateRequest">Module Create Request Model.</param>
        [HttpPost(Name = "CreateModule")]
        public async Task<ActionResult<ModuleResponse>> Create([FromBody] ModuleCreateRequest moduleCreateRequest)
        {
            _logger.LogInformationExtension(
                $"Create Module - Name: {moduleCreateRequest.Name}, Code: {moduleCreateRequest.Code}, Module Type: {moduleCreateRequest.ModuleTypeId}, Application: {moduleCreateRequest.ApplicationId}");

            var newModule = await _moduleService.Create(_mapper.Map<ModuleModel>(moduleCreateRequest));

            return Ok(new Models.Response<ModuleResponse>(_mapper.Map<ModuleResponse>(newModule)));
        }

        /// <summary>
        /// Update Module. 
        /// </summary>
        /// <param name="moduleUpdateRequest">Module Create Request Model.</param>
        [HttpPut(Name = "UpdateModule")]
        public async Task<ActionResult<ModuleResponse>> Update([FromBody] ModuleUpdateRequest moduleUpdateRequest)
        {
            _logger.LogInformationExtension(
                $"Update Module - Name: {moduleUpdateRequest.Name}, Code: {moduleUpdateRequest.Code}, Module Type: {moduleUpdateRequest.ModuleTypeId}, Application: {moduleUpdateRequest.ApplicationId}");

            await _moduleService.Update(_mapper.Map<ModuleModel>(moduleUpdateRequest));

            return Ok(new Models.Response<ModuleResponse>(null, true, string.Empty));
        }

        [HttpDelete("{id}", Name = "DeleteModule")]
        public async Task<ActionResult<ModuleResponse>> Delete(int id)
        {
            _logger.LogInformationExtension($"Delete Module - Id: {id}");

            await _moduleService.Delete(id);

            return Ok(new Models.Response<ModuleResponse>(null));
        }
    }
}
