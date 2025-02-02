﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _Repositories;
        public UnitOfWork(StoreContext dbContext)
        {
            _Repositories=new Hashtable();
            _dbContext = dbContext;
        }
        public async Task<int> CompleteAsync()
        {
         return  await  _dbContext.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
           return _dbContext.DisposeAsync();
        } 

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_Repositories.ContainsKey(type))
            {
                var Repository= new GenericRepository<TEntity>(_dbContext);
                _Repositories.Add(type, Repository);
                
            }
            return _Repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
