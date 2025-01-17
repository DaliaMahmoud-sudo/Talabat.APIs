using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpec :BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpec(ProductSpecParams Params) 
            : base(P=>(!Params.BrandId.HasValue || P.ProductBrandId== Params.BrandId) && (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId))
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(p=> p.Name);
                        break;

                }
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
        public ProductWithBrandAndTypeSpec(int id) : base(P=>P.Id==id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
