﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hektonian.DataSource.InMemory.Interfaces;
using Hektonian.DataSource.Interfaces;

#pragma warning disable 1998

namespace Hektonian.DataSource.InMemory
{
    internal class InMemoryDataSet<T> : IAsyncReadOnlyDataSet<T> where T : class
    {
        private readonly IInMemoryDataStore _store;
        private readonly IEnumerable<string> _includes;

        internal InMemoryDataSet(IInMemoryDataStore store, IEnumerable<string> includes)
        {
            _store = store;
            _includes = includes;
        }

        public async Task<IEnumerable<TOutput>> GetAllAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(
                    _store.Set<T>()
                          .AsQueryable()
                )
               .ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> condition)
        {
            return _store.Set<T>()
                         .AsQueryable()
                         .Where(condition)
                         .ToList();
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> condition)
        {
            return _store.Set<T>()
                         .AsQueryable()
                         .First(condition);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _store.Set<T>()
                         .AsQueryable()
                         .FirstOrDefault(condition);
        }

        public async Task<T> SingleAsync(Expression<Func<T, bool>> condition)
        {
            return _store.Set<T>()
                         .AsQueryable()
                         .Single(condition);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _store.Set<T>()
                         .AsQueryable()
                         .SingleOrDefault(condition);
        }

        public async Task<T> LastAsync(Expression<Func<T, bool>> condition)
        {
            return _store.Set<T>()
                         .AsQueryable()
                         .Last(condition);
        }

        public async Task<T> LastOrDefaultAsync(Expression<Func<T, bool>> condition)
        {
            return _store.Set<T>()
                         .AsQueryable()
                         .LastOrDefault(condition);
        }

        public async Task<TOutput> FirstAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(
                    _store.Set<T>()
                          .AsQueryable()
                )
               .First();
        }

        public async Task<TOutput> FirstOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(
                    _store.Set<T>()
                          .AsQueryable()
                )
               .FirstOrDefault();
        }

        public async Task<TOutput> SingleAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(
                    _store.Set<T>()
                          .AsQueryable()
                )
               .Single();
        }

        public async Task<TOutput> SingleOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(
                    _store.Set<T>()
                          .AsQueryable()
                )
               .SingleOrDefault();
        }

        public async Task<TOutput> LastAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(
                    _store.Set<T>()
                          .AsQueryable()
                )
               .Last();
        }

        public async Task<TOutput> LastOrDefaultAsync<TOutput>(Func<IQueryable<T>, IQueryable<TOutput>> queryBuilder)
        {
            return queryBuilder(
                    _store.Set<T>()
                          .AsQueryable()
                )
               .LastOrDefault();
        }
    }
}