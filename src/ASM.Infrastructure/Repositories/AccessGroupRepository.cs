using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Core.Specifications;
using ASM.Infrastructure.Data;
using ASM.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ASM.Infrastructure.Repositories
{
    public class AccessGroupRepository : Repository<AccessGroup>, IAccessGroupRepository
    {
        private readonly ASMContext _asmContext;

        public AccessGroupRepository(ASMContext asmContext) : base(asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }

        public async Task<IEnumerable<AccessGroup>> GetAll()
        {
            var spec = new AccessGroupSpecification();
            return await GetAsync(spec);
        }

        public async Task<AccessGroup> GetById(int id)
        {
            var spec = new AccessGroupSpecification(id);
            return (await GetAsync(spec)).FirstOrDefault();
        }

        public async Task<IEnumerable<AccessGroup>> GetByApplicationDepartment(Guid applicationId, int departmentId)
        {
            if (departmentId == 0) 
            {
                var spec = new AccessGroupSpecification(applicationId);
                return await GetAsync(spec);
            }
            else 
            {
                var spec = new AccessGroupSpecification(applicationId, departmentId);
                return await GetAsync(spec);
            }
        }

        public async Task<AccessGroup> Update(AccessGroup accessGroup)
        {
            await using var transaction = await _asmContext.Database.BeginTransactionAsync();

            var existingAccessGroup = await _asmContext.AccessGroups.Include(x => x.AccessGroupModulePermissions)
                .FirstOrDefaultAsync(x => x.AccessGroupId == accessGroup.AccessGroupId);

            if (existingAccessGroup == null)
                throw new ApplicationException($"Not able to find Access Group with id: {accessGroup.AccessGroupId}");

            existingAccessGroup.Name = accessGroup.Name;
            existingAccessGroup.Description = accessGroup.Description;
            existingAccessGroup.ApplicationId = accessGroup.ApplicationId;
            existingAccessGroup.DepartmentId = accessGroup.DepartmentId;
            existingAccessGroup.IsActive = accessGroup.IsActive;
            existingAccessGroup.LastUpdatedBy = accessGroup.LastUpdatedBy;
            existingAccessGroup.LastUpdated = DateTime.Now;

            //Find Modules needs to be deleted
            var deletedAccessGroupModulePermissions = existingAccessGroup.AccessGroupModulePermissions.Where(item =>
                !accessGroup.AccessGroupModulePermissions.Any(e =>
                    item.AccessGroupId == e.AccessGroupId && item.ModuleId == e.ModuleId) &&
                item.IsDeleted == false);
            foreach (var accessGroupModulePermission in deletedAccessGroupModulePermissions)
            {
                accessGroupModulePermission.IsDeleted = true;
                accessGroupModulePermission.LastUpdated = DateTime.Now;
                accessGroupModulePermission.LastUpdatedBy = accessGroup.LastUpdatedBy;
            }

            //Find Modules needs to be added
            var newAccessGroupModulePermissions = accessGroup.AccessGroupModulePermissions.Where(item =>
                !existingAccessGroup.AccessGroupModulePermissions.Any(e =>
                    item.AccessGroupId == e.AccessGroupId && item.ModuleId == e.ModuleId));

            foreach (var accessGroupModulePermission in newAccessGroupModulePermissions)
            {
                existingAccessGroup.AccessGroupModulePermissions.Add(new AccessGroupModulePermission
                {
                    AccessGroupId = accessGroupModulePermission.AccessGroupId,
                    ModuleId = accessGroupModulePermission.ModuleId,
                    HasViewAccess = accessGroupModulePermission.HasViewAccess,
                    HasCreateAccess = accessGroupModulePermission.HasCreateAccess,
                    HasUpdateAccess = accessGroupModulePermission.HasUpdateAccess,
                    HasDeleteAccess = accessGroupModulePermission.HasDeleteAccess,
                    HasAccessRight = accessGroupModulePermission.HasAccessRight,
                    Created = DateTime.Now,
                    CreatedBy = existingAccessGroup.LastUpdatedBy,
                    LastUpdated = DateTime.Now,
                    LastUpdatedBy = existingAccessGroup.LastUpdatedBy
                });
            }

            _asmContext.AccessGroups.Update(existingAccessGroup);

            await _asmContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return existingAccessGroup;
        }

        public async Task Delete(int id, int userId)
        {
            await using var transaction = await _asmContext.Database.BeginTransactionAsync();

            var existingAccessGroup = await _asmContext.AccessGroups.Include(x => x.AccessGroupModulePermissions)
                .Include(x => x.AccessGroupAssignments)
                .FirstOrDefaultAsync(x => x.AccessGroupId == id);

            if (existingAccessGroup == null)
                throw new ApplicationException($"Not able to find Access Group with id: {id}");

            existingAccessGroup.IsDeleted = true;
            existingAccessGroup.LastUpdated = DateTime.Now;
            existingAccessGroup.LastUpdatedBy = userId;

            foreach (var currentAccessGroupModulePermission in existingAccessGroup.AccessGroupModulePermissions)
            {
                currentAccessGroupModulePermission.IsDeleted = true;
                currentAccessGroupModulePermission.LastUpdated = DateTime.Now;
                currentAccessGroupModulePermission.LastUpdatedBy = userId;
            }

            foreach (var currentAccessGroupAssignment in existingAccessGroup.AccessGroupAssignments)
            {
                currentAccessGroupAssignment.IsDeleted = true;
                currentAccessGroupAssignment.LastUpdated = DateTime.Now;
                currentAccessGroupAssignment.LastUpdatedBy = userId;
            }

            _asmContext.AccessGroups.Update(existingAccessGroup);

            await _asmContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task<bool> IsAccessGroupExists(AccessGroup accessGroups)
        {
            AccessGroup existAccessGroup;
            if (accessGroups.AccessGroupId == 0)
            {
                existAccessGroup = await _asmContext.AccessGroups.FirstOrDefaultAsync(x =>
                    x.ApplicationId == accessGroups.ApplicationId && x.Name == accessGroups.Name &&
                    x.DepartmentId == accessGroups.DepartmentId && x.IsDeleted == false);
            }
            else
            {
                existAccessGroup = await _asmContext.AccessGroups.FirstOrDefaultAsync(x =>
                    x.AccessGroupId != accessGroups.AccessGroupId && x.ApplicationId == accessGroups.ApplicationId &&
                    x.Name == accessGroups.Name && x.DepartmentId == accessGroups.DepartmentId && x.IsDeleted == false);
            }

            return existAccessGroup != null;
        }
    }
}
