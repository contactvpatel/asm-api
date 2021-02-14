using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Business.Interfaces;
using ASM.Util.Logging;
using ASM.Util.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASM.Api.Controllers
{
    [Route("api/v{version:apiVersion}/roles")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RoleController : ControllerBase
    {
        private readonly IMisService _misService;
        private readonly ILogger<RoleController> _logger;
        private readonly IMapper _mapper;

        public RoleController(IMisService misService, ILogger<RoleController> logger, IMapper mapper)
        {
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return All Role List.
        /// </summary>                
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleModel>>> GetAll()
        {
            _logger.LogInformationExtension("Get All Roles");
            var roles = await _misService.GetAllRoles();
            string message;

            if (roles == null)
            {
                message = "No roles found";
                _logger.LogErrorExtension(message, null);
                return NotFound(new Models.Response<DepartmentModel>(false, message));
            }

            message = $"Found {roles.Count()} roles";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<RoleModel>>(_mapper.Map<IEnumerable<RoleModel>>(roles), message));
        }

        /// <summary>
        /// Return Roles By Department Id.
        /// </summary>
        /// <param name="departmentId">Department Id.</param>
        [HttpGet("{departmentId}", Name = "GetRoleByDepartmentId")]
        public async Task<ActionResult<IEnumerable<RoleModel>>> GetByDepartmentId(int departmentId)
        {
            _logger.LogInformationExtension($"Get Roles By Department Id: {departmentId}");
            var roles = await _misService.GetRoleByDepartmentId(departmentId);
            string message;

            if (roles == null)
            {
                message = $"No roles found by department id: {departmentId}";
                _logger.LogErrorExtension(message, null);
                return NotFound(new Models.Response<DepartmentModel>(false, message));
            }

            message = $"Found {roles.Count()} roles by department id: {departmentId}";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<RoleModel>>(_mapper.Map<IEnumerable<RoleModel>>(roles), message));
        }
    }
}
