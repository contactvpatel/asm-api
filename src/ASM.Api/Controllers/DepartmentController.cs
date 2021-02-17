using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Business.Interfaces;
using ASM.Util.Logging;
using ASM.Util.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ASM.Api.Controllers
{
    [Route("api/v{version:apiVersion}/departments")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMisService _misService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IMapper _mapper;

        public DepartmentController(IMisService misService, ILogger<DepartmentController> logger, IMapper mapper)
        {
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Return Department List.
        /// </summary>                
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetAll()
        {
            _logger.LogInformationExtension("Get All Departments");
            var departments = await _misService.GetAllDepartments();
            string message;

            if (departments == null)
            {
                message = "No departments found";
                _logger.LogErrorExtension(message, null);
                return NotFound(new Models.Response<DepartmentModel>(false, message));
            }

            message = $"Found {departments.Count()} departments";

            _logger.LogInformationExtension(message);

            return Ok(new Models.Response<IEnumerable<DepartmentModel>>(
                _mapper.Map<IEnumerable<DepartmentModel>>(departments), message));
        }

        /// <summary>
        /// Return Department By Id.
        /// </summary>
        /// <param name="id">Department Id.</param>
        [HttpGet("{id}", Name = "GetDepartmentById")]
        public async Task<ActionResult<DepartmentModel>> GetById(int id)
        {
            _logger.LogInformationExtension($"Get Department By Id: {id}");
            var department = await _misService.GetDepartmentById(id);
            if (department == null)
            {
                var message = $"No department found with id {id}";
                _logger.LogWarningExtension(message);
                return NotFound(new Models.Response<DepartmentModel>(false, message));
            }

            return Ok(new Models.Response<DepartmentModel>(_mapper.Map<DepartmentModel>(department)));
        }
    }
}
