using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository <T> where T : BaseEntity

    {
        #region without specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec);

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);

        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);


        //Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
    }
}
