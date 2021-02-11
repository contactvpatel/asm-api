using ASM.Api.Dto;
using ASM.Business.Models;
using AutoMapper;

namespace ASM.Api.Mapper
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            CreateMap<ModuleTypeModel, ModuleTypeResponse>().ReverseMap();
            CreateMap<ModuleModel, ModuleResponse>().ReverseMap();
            CreateMap<ModuleModel, ModuleCreateRequest>().ReverseMap();
            CreateMap<ModuleModel, ModuleUpdateRequest>().ReverseMap();
            CreateMap<ModuleHierachyModel, ModuleHierachyResponse>().ReverseMap();
            CreateMap<ModuleHierachyModel, ModuleHierarchyCreateRequest>().ReverseMap();
        }
    }
}
