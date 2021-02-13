﻿using System;
using ASM.Core.Entities;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Specifications
{
    public class ModuleSpecification : BaseSpecification<Module>
    {
        public ModuleSpecification() : base(b => b.IsDeleted == false)
        {
            AddInclude(x => x.ModuleType);
            AddInclude(x => x.ParentModule);
        }

        public ModuleSpecification(int id) : base(b => b.ModuleId == id)
        {
            AddInclude(x => x.ModuleType);
            AddInclude(x => x.ParentModule);
        }

        public ModuleSpecification(Guid applicationId) : base(b =>
            b.ApplicationId == applicationId && b.IsDeleted == false)
        {
            AddInclude(x => x.ModuleType);
            AddInclude(x => x.ParentModule);
        }

        public ModuleSpecification(bool isActive) : base(b => b.IsActive == isActive && b.IsDeleted == false)
        {
            AddInclude(x => x.ModuleType);
            AddInclude(x => x.ParentModule);
        }
    }
}
