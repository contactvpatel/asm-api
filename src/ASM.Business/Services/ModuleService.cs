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

        public async Task<IEnumerable<ModuleModel>> Get()
        {
            var modules = await _moduleRepository.Get();
            return ObjectMapper.Mapper.Map<IEnumerable<ModuleModel>>(modules);
        }

        public async Task<ModuleModel> GetById(int id)
        {
            var moduleById = await _moduleRepository.GetByIdAsync(id);
            return ObjectMapper.Mapper.Map<ModuleModel>(moduleById);
        }

        public async Task<IEnumerable<ModuleModel>> GetParentModules(int moduleId)
        {
            var moduleById = await _moduleRepository.GetParentModules(moduleId);
            return ObjectMapper.Mapper.Map<IEnumerable<ModuleModel>>(moduleById);
        }

        public async Task<ModuleModel> Create(ModuleModel module)
        {
            foreach (var hierarchyModel in module.ModuleHierarchyModules)
            {
                hierarchyModel.Created = DateTime.Now;
                hierarchyModel.CreatedBy = module.CreatedBy;
                hierarchyModel.LastUpdated = DateTime.Now;
                hierarchyModel.LastUpdatedBy = module.CreatedBy;
            }
            var newModule = await _moduleRepository.Create(ObjectMapper.Mapper.Map<Module>(module));
            _logger.LogInformationExtension(
                $"Module is successfully created. Module Id: {module.ModuleId}, Name: {module.Name}");
            return ObjectMapper.Mapper.Map<ModuleModel>(newModule);
        }

        public async Task Update(ModuleModel module)
        {
            foreach (var hierarchyModel in module.ModuleHierarchyModules)
            {
                hierarchyModel.Created = DateTime.Now;
                hierarchyModel.CreatedBy = module.CreatedBy;
                hierarchyModel.LastUpdated = DateTime.Now;
                hierarchyModel.LastUpdatedBy = module.CreatedBy;
            }

            await _moduleRepository.Update(ObjectMapper.Mapper.Map<Module>(module));
            _logger.LogInformationExtension(
                $"Module is successfully updated. Module Id: {module.ModuleId}, Name: {module.Name}");
        }

        public async Task Delete(ModuleModel module)
        {
            module.IsDeleted = true;
            module.LastUpdated = DateTime.Now;
            module.LastUpdatedBy = 1;
            await _moduleRepository.UpdateAsync(ObjectMapper.Mapper.Map<Module>(module));
        }

        public async Task<bool> IsModuleExists(ModuleModel module)
        {
            var mappedEntity = ObjectMapper.Mapper.Map<Module>(module);
            return await _moduleRepository.IsModuleExists(mappedEntity);
        }
    }
}
