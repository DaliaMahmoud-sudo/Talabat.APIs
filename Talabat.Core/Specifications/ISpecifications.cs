using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T>where T : BaseEntity
    {
        //where condition
        public Expression<Func<T, bool>> Criteria { get; set; }

        //list of includes
        public List<Expression<Func<T, Object>>> Includes { get; set; }   
    }
}
