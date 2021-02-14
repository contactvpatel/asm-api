using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Business.Models;

namespace ASM.Business.Interfaces
{
    public interface IModuleTypeService
    {
        Task<IEnumerable<ModuleTypeModel>> GetAll();
    }
}