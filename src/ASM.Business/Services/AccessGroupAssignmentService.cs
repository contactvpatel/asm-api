using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Business.Interfaces;
using ASM.Business.Mapper;
using ASM.Business.Models;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Util.Logging;
using Microsoft.Extensions.Logging;

namespace ASM.Business.Services
{
    public class AccessGroupAssignmentService : IAccessGroupAssignmentService
    {
        private readonly IAccessGroupAssignmentRepository _accessGroupAssignmentRepository;
        private readonly IMisService _misService;
        private readonly ISsoService _ssoService;
        private readonly ILogger<AccessGroupService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessGroupAssignmentRepository"></param>
        /// <param name="misService"></param>
        /// <param name="logger"></param>
        public AccessGroupAssignmentService(IAccessGroupAssignmentRepository accessGroupAssignmentRepository,
            IMisService misService,ISsoService ssoService,
            ILogger<AccessGroupService> logger)
        {
            _ssoService = ssoService ??
                                               throw new ArgumentNullException(nameof(ssoService));
            _accessGroupAssignmentRepository = accessGroupAssignmentRepository ??
                                               throw new ArgumentNullException(nameof(accessGroupAssignmentRepository));
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<AccessGroupAssignmentModel>> GetAll()
        {
            var accessGroupAssignments = await _accessGroupAssignmentRepository.GetAll();
            var departments = await _misService.GetAllDepartments();
            var roles = await _misService.GetAllRoles();
            var positions = await _misService.GetAllPositions();
            var applications = await _ssoService.GetAllApplications();
            return await Task.FromResult(
                from accessGroupAssignment in accessGroupAssignments
                join department in departments
                    on accessGroupAssignment.AccessGroup.DepartmentId equals department.DepartmentId into dep
                from department in dep.DefaultIfEmpty()
                join application in applications
                    on accessGroupAssignment.AccessGroup.ApplicationId equals application.ApplicationId
                join position in positions
                    on accessGroupAssignment.PositionId equals position.PositionId into position
                from pos in position.DefaultIfEmpty()
                join role in roles
                    on accessGroupAssignment.RoleId equals role.RoleId into role
                from rol in role.DefaultIfEmpty()
                select new AccessGroupAssignmentModel
                {
                    AccessGroupAssignmentId = accessGroupAssignment.AccessGroupAssignmentId,
                    AccessGroupId = accessGroupAssignment.AccessGroupId,
                    Name = accessGroupAssignment.AccessGroup.Name,
                    ApplicationId = accessGroupAssignment.AccessGroup.ApplicationId,
                    ApplicationName = application.ApplicationName,
                    DepartmentId = department?.DepartmentId ?? 0,
                    DepartmentName = department?.DepartmentName ?? string.Empty,
                    RoleId = accessGroupAssignment.RoleId,
                    RoleName = rol?.RoleName ?? string.Empty,
                    PositionId = accessGroupAssignment.PositionId,
                    PositionName = pos?.PositionName ?? string.Empty,
                    PersonId = accessGroupAssignment.PersonId,
                    LastUpdatedBy = accessGroupAssignment.LastUpdatedBy,
                });
        }

        public async Task Create(IEnumerable<AccessGroupAssignmentModel> accessGroupAssignments)
        {
            await _accessGroupAssignmentRepository.Create(
                ObjectMapper.Mapper.Map<IEnumerable<AccessGroupAssignment>>(accessGroupAssignments));

            _logger.LogInformationExtension(
                $"Access Group Assignment is successfully created. Access Group Id: {accessGroupAssignments.FirstOrDefault().AccessGroupId}");
        }

        public async Task Delete(int id, int userId)
        {
            var existingAccessGroupAssignment = await _accessGroupAssignmentRepository.GetByIdAsync(id);
            if (existingAccessGroupAssignment == null)
                throw new ApplicationException($"Not able to find Access Group Assignment with id: {id}");

            existingAccessGroupAssignment.IsDeleted = true;
            existingAccessGroupAssignment.LastUpdated = DateTime.Now;
            existingAccessGroupAssignment.LastUpdatedBy = userId;

            await _accessGroupAssignmentRepository.UpdateAsync(existingAccessGroupAssignment);

            _logger.LogInformationExtension(
                $"Access Group Assignment is successfully deleted. Access Group Assignment Id: {id}, User Id: {userId}");
        }
    }
}