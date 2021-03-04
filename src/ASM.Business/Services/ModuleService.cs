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
    public class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ISsoService _ssoService;
        private readonly ILogger<ModuleService> _logger;

        public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger,ISsoService ssoService)
        {
            _ssoService = ssoService ?? throw new ArgumentNullException(nameof(ssoService));
            _moduleRepository = moduleRepository ?? throw new ArgumentNullException(nameof(moduleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ModuleModel>> GetAll()
        {
            var modules = await _moduleRepository.GetAll();            
            var applications = await _ssoService.GetAllApplications();
            return await Task.FromResult(
                from module in modules
                join application in applications
                on module.ApplicationId equals application.ApplicationId into app
                from application in app.DefaultIfEmpty()
                select new ModuleModel
                {
                    ModuleId=module.ModuleId,
                    ApplicationId=module.ApplicationId,
                    ApplicationName = application.ApplicationName,
                    Code=module.Code,
                    Name=module.Name,
                    IsActive=module.IsActive,
                    ParentModule =
                        ObjectMapper.Mapper.Map<ModuleModel>(module.ParentModule),
                    ModuleType =
                        ObjectMapper.Mapper.Map<ModuleTypeModel>(module.ModuleType)
                });
        }

        public async Task<ModuleModel> GetById(int id)
        {
            return ObjectMapper.Mapper.Map<ModuleModel>(await _moduleRepository.GetById(id));
        }

        public async Task<IEnumerable<ModuleModel>> GetByApplicationId(Guid applicationId)
        {
            return ObjectMapper.Mapper.Map<IEnumerable<ModuleModel>>(
                await _moduleRepository.GetByApplicationId(applicationId));
        }

        public async Task<ModuleModel> Create(ModuleModel module)
        {
            var isModuleExists = await _moduleRepository.IsModuleExists(ObjectMapper.Mapper.Map<Module>(module));
            if (isModuleExists)
                throw new ApplicationException(
                    $"Module combination already exists. Name: {module.Name}, Code: {module.Code}, Module Type: {module.ModuleTypeId}, Application: {module.ApplicationId}");

            var newModule = await _moduleRepository.AddAsync(ObjectMapper.Mapper.Map<Module>(module));

            _logger.LogInformationExtension(
                $"Module is successfully created. Module Id: {module.ModuleId}, Name: {module.Name}");

            return ObjectMapper.Mapper.Map<ModuleModel>(newModule);
        }

        public async Task Update(ModuleModel module)
        {
            var isModuleExists = await _moduleRepository.IsModuleExists(ObjectMapper.Mapper.Map<Module>(module));
            if (isModuleExists)
                throw new ApplicationException(
                    $"Module combination already exists. Name: {module.Name}, Code: {module.Code}, Module Type: {module.ModuleTypeId}, Application: {module.ApplicationId}");

            var existingModule = await _moduleRepository.GetByIdAsync(module.ModuleId);
            if (existingModule == null || existingModule.IsDeleted)
                throw new ApplicationException($"Not able to find Module with id: {module.ModuleId}");

            existingModule.Name = module.Name;
            existingModule.Code = module.Code;
            existingModule.ModuleTypeId = module.ModuleTypeId;
            existingModule.ApplicationId = module.ApplicationId;
            existingModule.ParentModuleId = module.ParentModuleId == 0 ? null : module.ParentModuleId;
            existingModule.IsActive = module.IsActive;
            existingModule.LastUpdatedBy = module.LastUpdatedBy;

            await _moduleRepository.UpdateAsync(existingModule);

            _logger.LogInformationExtension(
                $"Module is successfully updated. Module Id: {module.ModuleId}, Name: {module.Name}");
        }

        public async Task Delete(int id, int userId)
        {
            await _moduleRepository.Delete(id, userId);
            _logger.LogInformationExtension($"Module is successfully deleted. Module Id: {id}, User Id: {userId}");
        }

        public async Task<bool> IsModuleExists(ModuleModel module)
        {
            return await _moduleRepository.IsModuleExists(ObjectMapper.Mapper.Map<Module>(module));
        }
    }
}
