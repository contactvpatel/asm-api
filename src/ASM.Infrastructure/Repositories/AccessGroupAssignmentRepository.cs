using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASM.Core.Entities;
using ASM.Core.Repositories;
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

        public Task<IEnumerable<AccessGroupAssignment>> GetAccessGroupAssignment()
        {
            throw new NotImplementedException();
        }
    }
}