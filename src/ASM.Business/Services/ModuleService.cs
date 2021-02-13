using System;
using System.Collections.Generic;
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
        private readonly ILogger<ModuleService> _logger;

        public ModuleService(IModuleRepository moduleRepository, ILogger<ModuleService> logger)
        {
            _moduleRepository = moduleRepository ?? throw new ArgumentNullException(nameof(moduleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ModuleModel>> GetAll()
        {
            var modules = await _moduleRepository.GetAll();
            return ObjectMapper.Mapper.Map<IEnumerable<ModuleModel>>(modules);
        }

        public async Task<ModuleModel> GetById(int id)
        {
            var moduleById = await _moduleRepository.GetById(id);
            return ObjectMapper.Mapper.Map<ModuleModel>(moduleById);
        }

        public async Task<IEnumerable<ModuleModel>> GetByApplicationId(Guid applicationId)
        {
            var modules = await _moduleRepository.GetByApplicationId(applicationId);
            return ObjectMapper.Mapper.Map<IEnumerable<ModuleModel>>(modules);
        }

        public async Task<ModuleModel> Create(ModuleModel module)
        {
            var isModuleExists = await _moduleRepository.IsModuleExists(ObjectMapper.Mapper.Map<Module>(module));
            if (isModuleExists)
                throw new ApplicationException(
                    $"Module combination already exists. Name: {module.Name}, Code: {module.Code}, Module Type: {module.ModuleTypeId}, Application: {module.ApplicationId}");

            module.Created = DateTime.Now;
            module.LastUpdated = DateTime.Now;
            module.LastUpdatedBy = module.CreatedBy;

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
            if (existingModule == null)
                throw new ApplicationException($"Not able to find Module with id: {module.ModuleId}");

            existingModule.Name = module.Name;
            existingModule.Code = module.Code;
            existingModule.ModuleTypeId = module.ModuleTypeId;
            existingModule.ApplicationId = module.ApplicationId;
            existingModule.ParentModuleId = module.ParentModuleId == 0 ? null : module.ParentModuleId;
            existingModule.IsActive = module.IsActive;
            existingModule.LastUpdatedBy = module.LastUpdatedBy;
            existingModule.LastUpdated = DateTime.Now;

            await _moduleRepository.UpdateAsync(existingModule);
            _logger.LogInformationExtension(
                $"Module is successfully updated. Module Id: {module.ModuleId}, Name: {module.Name}");
        }

        public async Task Delete(int id)
        {
            var existingModule = await _moduleRepository.GetByIdAsync(id);
            if (existingModule == null)
            {
                string message = $"Not able to find Module with id: {id}";
                _logger.LogErrorExtension(message, null);
                throw new ApplicationException(message);
            }

            existingModule.IsDeleted = true;
            existingModule.LastUpdated = DateTime.Now;
            existingModule.LastUpdatedBy = 1;
            await _moduleRepository.UpdateAsync(existingModule);
        }

        public async Task<bool> IsModuleExists(ModuleModel module)
        {
            var mappedEntity = ObjectMapper.Mapper.Map<Module>(module);
            return await _moduleRepository.IsModuleExists(mappedEntity);
        }
    }
}
