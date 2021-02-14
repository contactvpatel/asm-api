﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASM.Core.Entities.Base;
using ASM.Core.Specifications.Base;

namespace ASM.Core.Repositories.Base
{
    public interface IRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> condition);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            string includeString = null,
            bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec);

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<int> CountAsync(ISpecification<T> spec);
    }
}