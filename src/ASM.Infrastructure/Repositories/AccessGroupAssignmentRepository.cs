using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories;
using ASM.Core.Specifications;
using ASM.Infrastructure.Repositories.Base;

namespace ASM.Infrastructure.Repositories
{
    public class AccessGroupAssignmentRepository : Repository<AccessGroupAssignment>, IAccessGroupAssignmentRepository
    {
        private readonly Data.ASMContext _asmContext;

        public AccessGroupAssignmentRepository(Data.ASMContext asmContext) : base(asmContext)
        {
            _asmContext = asmContext ?? throw new ArgumentNullException(nameof(asmContext));
        }

        public async Task<IEnumerable<AccessGroupAssignment>> GetAll()
        {
            var spec = new AccessGroupAssignmentSpecification();
            return await GetAsync(spec);
        }

        public async Task Create(IEnumerable<AccessGroupAssignment> accessGroupAssignments)
        {
            var accessGroupId = accessGroupAssignments.FirstOrDefault()?.AccessGroupId;

            var existingAccessGroupAssignments =
                _asmContext.AccessGroupAssignments.Where(x => x.AccessGroupId == accessGroupId);

            var addNewAccessGroupAssignments = accessGroupAssignments.Where(item =>
                !existingAccessGroupAssignments.Any(e =>
                    item.AccessGroupId == e.AccessGroupId && item.RoleId == e.RoleId &&
                    item.PositionId == e.PositionId && item.PersonId == e.PersonId && e.IsDeleted == false));

            await _asmContext.AccessGroupAssignments.AddRangeAsync(addNewAccessGroupAssignments);

            await _asmContext.SaveChangesAsync();
        }
    }
}