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
using IMisService = ASM.Core.Services.IMisService;

namespace ASM.Business.Services
{
    public class AccessGroupService : IAccessGroupService
    {
        private readonly IAccessGroupRepository _accessGroupRepository;
        private readonly IMisService _misService;
        private readonly ILogger<AccessGroupService> _logger;
        private readonly ISsoService _ssoService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessGroupRepository"></param>
        /// <param name="misService"></param>
        /// <param name="logger"></param>
        public AccessGroupService(IAccessGroupRepository accessGroupRepository, IMisService misService,
            ILogger<AccessGroupService> logger, ISsoService ssoService)
        {
            _ssoService = ssoService ??
                                     throw new ArgumentNullException(nameof(ssoService));
            _accessGroupRepository = accessGroupRepository ??
                                     throw new ArgumentNullException(nameof(accessGroupRepository));
            _misService = misService ?? throw new ArgumentNullException(nameof(misService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<AccessGroupModel>> GetAll()
        {
            var accessGroups = await _accessGroupRepository.GetAll();
            var departments = await _misService.GetAllDepartments();
            var application = await _ssoService.GetAllApplications();

            return await Task.FromResult(
                from accessGroup in accessGroups                
                join applications in application
                    on accessGroup.ApplicationId equals applications.ApplicationId
                join department in departments
                on accessGroup.DepartmentId equals department.DepartmentId into departmentJoin
                from depart in departmentJoin.DefaultIfEmpty()
                select new AccessGroupModel
                {
                    AccessGroupId = accessGroup.AccessGroupId,
                    Name = accessGroup.Name,
                    Description = accessGroup.Description,
                    ApplicationName=applications.ApplicationName,
                    ApplicationId = accessGroup.ApplicationId,
                    DepartmentId = depart?.DepartmentId ?? 0,
                    DepartmentName = depart?.DepartmentName ?? string.Empty,
                    LastUpdated = accessGroup.LastUpdated,
                    IsActive = accessGroup.IsActive,
                    CreatedBy = accessGroup.CreatedBy,
                    LastUpdatedBy = accessGroup.LastUpdatedBy,
                    AccessGroupModulePermissions =
                        ObjectMapper.Mapper.Map<ICollection<AccessGroupModulePermissionModel>>(accessGroup
                            .AccessGroupModulePermissions)
                });
        }

        public async Task<AccessGroupModel> GetById(int id)
        {
            return ObjectMapper.Mapper.Map<AccessGroupModel>(await _accessGroupRepository.GetById(id));
        }

        public async Task<IEnumerable<AccessGroupModel>> GetByApplicationDepartment(Guid applicationId,
            int departmentId)
        {
            return ObjectMapper.Mapper.Map<IEnumerable<AccessGroupModel>>(
                await _accessGroupRepository.GetByApplicationDepartment(applicationId, departmentId));
        }

        public async Task<AccessGroupModel> Create(AccessGroupModel accessGroupModel)
        {
            var isAccessGroupExists =
                await _accessGroupRepository.IsAccessGroupExists(
                    ObjectMapper.Mapper.Map<AccessGroup>(accessGroupModel));

            if (isAccessGroupExists)
                throw new ApplicationException(
                    $"Access Group combination already exists. Name: {accessGroupModel.Name}, Application Id: {accessGroupModel.ApplicationId}, Department Id: {accessGroupModel.DepartmentId}");

            accessGroupModel.Created = DateTime.Now;
            accessGroupModel.LastUpdated = DateTime.Now;

            accessGroupModel.AccessGroupModulePermissions?.ToList().ForEach(x =>
            {
                x.Created = DateTime.Now;
                x.CreatedBy = accessGroupModel.CreatedBy;
                x.LastUpdated = DateTime.Now;
                x.LastUpdatedBy = accessGroupModel.LastUpdatedBy;
            });

            var newAccessGroup =
                await _accessGroupRepository.AddAsync(ObjectMapper.Mapper.Map<AccessGroup>(accessGroupModel));

            _logger.LogInformationExtension(
                $"Access Group is successfully created. Access Group Id: {accessGroupModel.AccessGroupId}, Name: {accessGroupModel.Name}");

            return ObjectMapper.Mapper.Map<AccessGroupModel>(newAccessGroup);
        }

        public async Task<AccessGroupModel> Update(AccessGroupModel accessGroupModel)
        {
            var isAccessGroupExists =
                await _accessGroupRepository.IsAccessGroupExists(
                    ObjectMapper.Mapper.Map<AccessGroup>(accessGroupModel));

            if (isAccessGroupExists)
                throw new ApplicationException(
                    $"Access Group combination already exists. Name: {accessGroupModel.Name}, Application Id: {accessGroupModel.ApplicationId}, Department Id: {accessGroupModel.DepartmentId}");

            var updatedAccessGroup =
                await _accessGroupRepository.Update(ObjectMapper.Mapper.Map<AccessGroup>(accessGroupModel));

            _logger.LogInformationExtension(
                $"Access Group is successfully updated. Access Group Id: {accessGroupModel.AccessGroupId}, Name: {accessGroupModel.Name}");

            return ObjectMapper.Mapper.Map<AccessGroupModel>(updatedAccessGroup);
        }

        public async Task Delete(int id, int userId)
        {
            await _accessGroupRepository.Delete(id, userId);
            _logger.LogInformationExtension(
                $"Access Group is successfully deleted. Access Group Id: {id}, User Id: {userId}");
        }

        public async Task<bool> IsAccessGroupExists(AccessGroupModel module)
        {
            return await _accessGroupRepository.IsAccessGroupExists(ObjectMapper.Mapper.Map<AccessGroup>(module));
        }
    }
}