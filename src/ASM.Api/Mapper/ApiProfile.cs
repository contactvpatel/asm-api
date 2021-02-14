using ASM.Api.Dto;
using ASM.Business.Models;
using AutoMapper;

namespace ASM.Api.Mapper
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            // Module Types
            CreateMap<ModuleTypeModel, ModuleTypeResponse>();

            // Modules
            CreateMap<ModuleCreateRequest, ModuleModel>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId));
            CreateMap<ModuleUpdateRequest, ModuleModel>()
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.UserId));
            CreateMap<ModuleModel, ModuleResponse>().ReverseMap();

            // Access Groups
            CreateMap<AccessGroupCreateRequest, AccessGroupModel>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId));
            CreateMap<AccessGroupUpdateRequest, AccessGroupModel>()
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.UserId));
            CreateMap<AccessGroupModel, AccessGroupResponse>().ReverseMap();

            // Access Group Module Permissions
            CreateMap<AccessGroupModulePermissionCreateRequest, AccessGroupModulePermissionModel>().ReverseMap();
            CreateMap<AccessGroupModulePermissionUpdateRequest, AccessGroupModulePermissionModel>().ReverseMap();
            CreateMap<AccessGroupModulePermissionModel, AccessGroupModulePermissionResponse>().ReverseMap();

            // Access Group Assignments
            CreateMap<AccessGroupAssignmentModel, AccessGroupAssignmentRequest>().ReverseMap();
            CreateMap<AccessGroupAssignmentModel, AccessGroupAssignmentResponse>().ReverseMap();
        }
    }
}
