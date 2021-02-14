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
    /// Access Group Controller. 
    /// Contain Access Group API. 
    /// </summary>
    [Route("api/v{version:apiVersion}/access-groups")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccessGroupController : ControllerBase
    {
        private readonly IAccessGroupService _accessGroupService;
        private readonly ILogger<AccessGroupController> _logger;
        private readonly IMapper _mapper;

        public AccessGroupController(IAccessGroupService accessGroupService, ILogger<AccessGroupController> logger,
            IMapper mapper)
        {
            _accessGroupService = accessGroupService ?? throw new ArgumentNullException(nameof(accessGroupService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return Access Group List.
        /// </summary>        
        [HttpGet(Name = "GetAllAccessGroups")]
        public async Task<ActionResult<IEnumerable<AccessGroupResponse>>> GetAll()
        {
            _logger.LogInformationExtension("Get All Access Groups");
            var accessGroups = await _accessGroupService.GetAll();
            string message;
            if (accessGroups == null)
            {
                message = "No access groups found";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<AccessGroupResponse>(false, message));
            }

            message = $"Found {accessGroups.Count()} access groups";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<AccessGroupResponse>>(
                _mapper.Map<IEnumerable<AccessGroupResponse>>(accessGroups), message));
        }

        /// <summary>
        /// Return Access Group By Id.
        /// </summary>
        /// <param name="id">Access Group Id.</param>
        [HttpGet("{id}", Name = "GetAccessGroupById")]
        public async Task<ActionResult<AccessGroupResponse>> GetById(int id)
        {
            _logger.LogInformationExtension($"Get Access Group By Id: {id}");
            var accessGroup = await _accessGroupService.GetById(id);
            if (accessGroup == null)
            {
                var message = $"No access group found with id {id}";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<AccessGroupResponse>(false, message));
            }

            return Ok(new Models.Response<AccessGroupResponse>(_mapper.Map<AccessGroupResponse>(accessGroup)));
        }

        /// <summary>
        /// Return Access Group By Application Id & Department Id.
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <param name="departmentId">Department Id</param>
        [Route("{applicationId}/{departmentId}", Name = "GetAccessGroupByApplicationDepartment")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessGroupResponse>>> GetByApplicationDepartment(Guid applicationId,
            int departmentId)
        {
            _logger.LogInformationExtension(
                $"GetAll Access Group. Application Id: {applicationId}, Department Id: {departmentId}");
            var accessGroups = await _accessGroupService.GetByApplicationDepartment(applicationId, departmentId);
            string message;
            if (accessGroups == null)
            {
                message =
                    $"No access group found with Application Id: {applicationId}, Department Id: {departmentId}";
                _logger.LogWarningExtension(message);
                return NotFound(
                    new Models.Response<Task<ActionResult<IEnumerable<AccessGroupResponse>>>>(false, message));
            }

            message =
                $"Found {accessGroups.Count()} access groups by Application Id: {applicationId}, Department Id: {departmentId}";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<AccessGroupResponse>>(
                _mapper.Map<IEnumerable<AccessGroupResponse>>(accessGroups), message));
        }

        /// <summary>
        /// Create Access Group. 
        /// </summary>
        /// <param name="accessGroupCreateRequest">Access Group Create Request Model.</param>
        [HttpPost(Name = "CreateAccessGroup")]
        public async Task<ActionResult<AccessGroupResponse>> Create([FromBody] AccessGroupCreateRequest accessGroupCreateRequest)
        {
            _logger.LogInformationExtension(
                $"Create Access Group - Name: {accessGroupCreateRequest.Name}, Application Id: {accessGroupCreateRequest.ApplicationId}, Department Id: {accessGroupCreateRequest.DepartmentId}");

            var newAccessGroup =
                await _accessGroupService.Create(_mapper.Map<AccessGroupModel>(accessGroupCreateRequest));

            return Ok(new Models.Response<AccessGroupResponse>(_mapper.Map<AccessGroupResponse>(newAccessGroup), "Access Group is successfully created."));
        }

        /// <summary>
        /// Update Access Group. 
        /// </summary>
        /// <param name="accessGroupUpdateRequest">Access Group Update Request Model.</param>
        [HttpPut(Name = "UpdateAccessGroup")]
        public async Task<ActionResult<AccessGroupResponse>> Update(
            [FromBody] AccessGroupUpdateRequest accessGroupUpdateRequest)
        {
            _logger.LogInformationExtension(
                $"Update Access Group - Name: {accessGroupUpdateRequest.Name}, Application Id: {accessGroupUpdateRequest.ApplicationId}, Department Id: {accessGroupUpdateRequest.DepartmentId}");

            await _accessGroupService.Update(_mapper.Map<AccessGroupModel>(accessGroupUpdateRequest));

            return Ok(new Models.Response<AccessGroupResponse>(null, true, "Access Group is successfully updated."));
        }

        /// <summary>
        /// Delete Access Group. 
        /// </summary>
        /// <param name="id">Access Group Id</param>
        /// <param name="userId">User Id</param>
        [HttpDelete("{id}/{userId}", Name = "DeleteAccessGroup")]
        public async Task<ActionResult<ModuleResponse>> Delete(int id, int userId)
        {
            _logger.LogInformationExtension($"Delete Access Group - Id: {id}, User Id: {userId}");

            await _accessGroupService.Delete(id, userId);

            return Ok(new Models.Response<ModuleResponse>(true, "Access Group is successfully deleted."));
        }
    }
}
