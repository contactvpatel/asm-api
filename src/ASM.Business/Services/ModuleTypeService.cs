using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Business.Interfaces;
using ASM.Business.Mapper;
using ASM.Business.Models;
using ASM.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace ASM.Business.Services
{
    public class ModuleTypeService : IModuleTypeService
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly ILogger<ModuleTypeService> _logger;

        public ModuleTypeService(IModuleTypeRepository moduleTypeRepository, ILogger<ModuleTypeService> logger)
        {
            _moduleTypeRepository =
                moduleTypeRepository ?? throw new ArgumentNullException(nameof(moduleTypeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ModuleTypeModel>> Get()
        {
            var moduleType = await _moduleTypeRepository.GetAllAsync();
            return ObjectMapper.Mapper.Map<IEnumerable<ModuleTypeModel>>(moduleType);
        }
    }
}
