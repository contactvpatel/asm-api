using System;
using System.Collections.Generic;
using System.Linq;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Infrastructure.Data;
using ASM.Util.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM.Infrastructure.Repositories
{
    public class ApplicationSecurityRepository : IApplicationSecurityRepository
    {
        private readonly ASMContext _asmContext;

        public ApplicationSecurityRepository(ASMContext asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }

        public IEnumerable<ApplicationSecurityModel> Get(Guid applicationId, int? roleId, int? positionId,
            int? personId)
        {
            var applicationSecurityModels = new List<ApplicationSecurityModel>();

            var assignedAccessGroupModulePermissions = new List<AccessGroupModulePermission>();

            var accessGroupsByRole = _asmContext.AccessGroupAssignments
                .Include(x => x.AccessGroup.AccessGroupModulePermissions)
                .Where(x =>
                    x.AccessGroup.ApplicationId == applicationId && x.RoleId == roleId && x.PositionId == null &&
                    x.PersonId == null && x.IsDeleted == false);

            foreach (var currentAccessGroupsByRole in accessGroupsByRole)
            {
                assignedAccessGroupModulePermissions.AddRange(currentAccessGroupsByRole.AccessGroup
                    .AccessGroupModulePermissions.Where(x => x.IsDeleted == false));
            }

            var accessGroupsByPosition = _asmContext.AccessGroupAssignments
                .Include(x => x.AccessGroup.AccessGroupModulePermissions)
                .Where(x =>
                    x.AccessGroup.ApplicationId == applicationId && x.RoleId == roleId && x.PositionId == positionId &&
                    x.PersonId == null &&
                    x.IsDeleted == false);

            foreach (var currentAccessGroupsByPosition in accessGroupsByPosition)
            {
                assignedAccessGroupModulePermissions.AddRange(currentAccessGroupsByPosition.AccessGroup
                    .AccessGroupModulePermissions.Where(x => x.IsDeleted == false));
            }

            var accessGroupsByPerson = _asmContext.AccessGroupAssignments
                .Include(x => x.AccessGroup.AccessGroupModulePermissions)
                .Where(x =>
                    x.AccessGroup.ApplicationId == applicationId && x.RoleId == null && x.PositionId == null &&
                    x.PersonId == personId &&
                    x.IsDeleted == false);

            foreach (var currentAccessGroupsByPerson in accessGroupsByPerson)
            {
                assignedAccessGroupModulePermissions.AddRange(currentAccessGroupsByPerson.AccessGroup
                    .AccessGroupModulePermissions.Where(x => x.IsDeleted == false));
            }

            var modules = new List<Module>();

            if (assignedAccessGroupModulePermissions.Any())
            {
                modules = _asmContext.Modules
                    .Where(x => x.ApplicationId == applicationId && x.IsDeleted == false && x.IsActive == true)
                    .Include(x => x.ModuleType)
                    .ToList();
            }

            foreach (var currentAccessGroupModulePermission in assignedAccessGroupModulePermissions)
            {
                var existingApplicationSecurityModel =
                    applicationSecurityModels.FirstOrDefault(x =>
                        x.ModuleId == currentAccessGroupModulePermission.ModuleId);

                if (existingApplicationSecurityModel == null)
                {
                    var currentModule = modules.FirstOrDefault(x => x.ModuleId == currentAccessGroupModulePermission.ModuleId);
                    if (currentModule != null)
                    {
                        applicationSecurityModels.Add(new ApplicationSecurityModel
                        {
                            ModuleId = currentAccessGroupModulePermission.ModuleId,
                            ModuleName = currentModule.Name,
                            ModuleCode = currentModule.Code,
                            IsControlType = currentModule.ModuleType.IsControlType,
                            HasViewAccess = currentAccessGroupModulePermission.HasViewAccess,
                            HasCreateAccess = currentAccessGroupModulePermission.HasCreateAccess,
                            HasUpdateAccess = currentAccessGroupModulePermission.HasUpdateAccess,
                            HasDeleteAccess = currentAccessGroupModulePermission.HasDeleteAccess,
                            HasAccessRight = currentAccessGroupModulePermission.HasAccessRight
                        });
                    }
                }
                else
                {
                    if (currentAccessGroupModulePermission.HasViewAccess)
                        existingApplicationSecurityModel.HasViewAccess =
                            currentAccessGroupModulePermission.HasViewAccess;

                    if (currentAccessGroupModulePermission.HasCreateAccess)
                        existingApplicationSecurityModel.HasCreateAccess =
                            currentAccessGroupModulePermission.HasCreateAccess;

                    if (currentAccessGroupModulePermission.HasUpdateAccess)
                        existingApplicationSecurityModel.HasUpdateAccess =
                            currentAccessGroupModulePermission.HasUpdateAccess;

                    if (currentAccessGroupModulePermission.HasDeleteAccess)
                        existingApplicationSecurityModel.HasDeleteAccess =
                            currentAccessGroupModulePermission.HasDeleteAccess;

                    if (currentAccessGroupModulePermission.HasAccessRight)
                        existingApplicationSecurityModel.HasAccessRight =
                            currentAccessGroupModulePermission.HasAccessRight;
                }
            }

            return applicationSecurityModels;
        }
    }
}