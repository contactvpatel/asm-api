using System;
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
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.UserId));

            CreateMap<ModuleUpdateRequest, ModuleModel>()
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.UserId));

            CreateMap<ModuleModel, ModuleResponse>().ReverseMap();

            // Access Groups
            CreateMap<AccessGroupCreateRequest, AccessGroupModel>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.UserId));
            CreateMap<AccessGroupUpdateRequest, AccessGroupModel>()
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.UserId));
            CreateMap<AccessGroupModel, AccessGroupResponse>().ReverseMap();

            // Access Group Module Permissions
            CreateMap<AccessGroupModulePermissionCreateRequest, AccessGroupModulePermissionModel>().ReverseMap();
            CreateMap<AccessGroupModulePermissionUpdateRequest, AccessGroupModulePermissionModel>().ReverseMap();
            CreateMap<AccessGroupModulePermissionModel, AccessGroupModulePermissionResponse>().ReverseMap();

            // Access Group Assignments
            CreateMap<AccessGroupAssignmentRequest, AccessGroupAssignmentModel>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(src => src.UserId));

            CreateMap<AccessGroupAssignmentModel, AccessGroupAssignmentResponse>().ReverseMap();
        }
    }
}
