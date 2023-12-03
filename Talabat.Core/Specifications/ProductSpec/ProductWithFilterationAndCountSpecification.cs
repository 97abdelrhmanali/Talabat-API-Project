using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpec
{
    public class ProductWithFilterationAndCountSpecification : BaseSpecifications<Product>
    {
        public ProductWithFilterationAndCountSpecification(ProductSpecParams SpecParams): 
            base(P =>
                (string.IsNullOrEmpty(SpecParams.Search) || P.Name.ToLower().Contains(SpecParams.Search)) &&
                (!SpecParams.BrandId.HasValue || P.BrandId == SpecParams.BrandId.Value)&&
                (!SpecParams.CategoryId.HasValue || P.CategoryId == SpecParams.CategoryId.Value)
            ) 
        {

        }
    }
}
