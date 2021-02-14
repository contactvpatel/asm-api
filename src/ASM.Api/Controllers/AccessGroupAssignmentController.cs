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
    /// Access Group Assignment Controller. 
    /// Contain Access Group Assignment API. 
    /// </summary>
    [Route("api/v{version:apiVersion}/access-group-assignments")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccessGroupAssignmentController : ControllerBase
    {
        private readonly IAccessGroupAssignmentService _accessGroupAssignmentService;
        private readonly ILogger<AccessGroupAssignmentController> _logger;
        private readonly IMapper _mapper;

        public AccessGroupAssignmentController(IAccessGroupAssignmentService accessGroupAssignmentService,
            ILogger<AccessGroupAssignmentController> logger,
            IMapper mapper)
        {
            _accessGroupAssignmentService = accessGroupAssignmentService ??
                                            throw new ArgumentNullException(nameof(accessGroupAssignmentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return Access Group Assignment List.
        /// </summary>        
        [HttpGet(Name = "GetAllAccessGroupAssignments")]
        public async Task<ActionResult<IEnumerable<AccessGroupAssignmentResponse>>> GetAll()
        {
            _logger.LogInformationExtension("GetAll All Access Group Assignments");
            var accessGroupAssignments = await _accessGroupAssignmentService.GetAll();
            string message;
            if (accessGroupAssignments == null)
            {
                message = "No access group assignments found";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<AccessGroupAssignmentResponse>(false, message));
            }

            message = $"Found {accessGroupAssignments.Count()} access group assignments";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<AccessGroupAssignmentResponse>>(
                _mapper.Map<IEnumerable<AccessGroupAssignmentResponse>>(accessGroupAssignments), message));
        }

        /// <summary>
        /// Create Access Group Assignment. 
        /// </summary>
        /// <param name="accessGroupAssignmentRequests">Access Group Assignment Request Model.</param>
        [HttpPost(Name = "CreateAccessGroupAssignment")]
        public async Task<ActionResult<AccessGroupResponse>> Create(
            [FromBody] IEnumerable<AccessGroupAssignmentRequest> accessGroupAssignmentRequests)
        {
            _logger.LogInformationExtension(
                $"Create Access Group Assignment - Access Group Id: {accessGroupAssignmentRequests.FirstOrDefault()?.AccessGroupId}");

            await _accessGroupAssignmentService.Create(
                _mapper.Map<IEnumerable<AccessGroupAssignmentModel>>(accessGroupAssignmentRequests));

            return Ok(
                new Models.Response<AccessGroupResponse>(null, "Access Group Assignment is successfully created."));
        }

        /// <summary>
        /// Delete Access Group Assignment. 
        /// </summary>
        /// <param name="id">Access Group Assignment Id</param>
        /// <param name="userId">User Id</param>
        [HttpDelete("{id}/{userId}", Name = "DeleteAccessGroupAssignment")]
        public async Task<ActionResult<ModuleResponse>> Delete(int id, int userId)
        {
            _logger.LogInformationExtension($"Delete Access Group Assignment - Id: {id}, User Id: {userId}");

            await _accessGroupAssignmentService.Delete(id, userId);

            return Ok(new Models.Response<ModuleResponse>(true, "Access Group Assignment is successfully deleted."));
        }
    }
}
