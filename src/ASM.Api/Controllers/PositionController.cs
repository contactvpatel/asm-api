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
    [Route("api/v{version:apiVersion}/positions")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PositionController : ControllerBase
    {
        private readonly IMisService _misService;
        private readonly ILogger<PositionController> _logger;
        private readonly IMapper _mapper;

        public PositionController(IMisService misService, ILogger<PositionController> logger, IMapper mapper)
        {
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return Positions By Role Id.
        /// </summary>
        /// <param name="roleId">Role Id.</param>
        [HttpGet("{roleId}", Name = "GetByRoleId")]
        public async Task<ActionResult<IEnumerable<PositionModel>>> GetByRoleId(int roleId)
        {
            _logger.LogInformationExtension($"Get Positions By Role Id: {roleId}");
            var positions = await _misService.GetPositions(roleId);
            string message;

            if (positions == null)
            {
                message = $"No positions found by role id: {roleId}";
                _logger.LogErrorExtension(message, null);
                return NotFound(new Models.Response<PositionModel>(false, message));
            }

            message = $"Found {positions.Count()} positions by role id: {roleId}";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<PositionModel>>(
                _mapper.Map<IEnumerable<PositionModel>>(positions),
                message));
        }
    }
}
