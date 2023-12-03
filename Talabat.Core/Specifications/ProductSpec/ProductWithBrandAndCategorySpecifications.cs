using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpec
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams SpecParams) :
            base(p => 
                    (string.IsNullOrEmpty(SpecParams.Search) || p.Name.ToLower().Contains(SpecParams.Search)) &&
                    (!SpecParams.BrandId.HasValue || p.BrandId == SpecParams.BrandId.Value) &&
                    (!SpecParams.CategoryId.HasValue || p.CategoryId == SpecParams.CategoryId.Value)
                )
        {
            AddingBrandsAndCategory();

            if (!string.IsNullOrEmpty(SpecParams.Sort))
            {
                switch (SpecParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            else
                AddOrderBy(P => P.Name);

            // size = 5
            // pages = 2 
            ApplyPagination((SpecParams.PageIndex - 1) * SpecParams.PageSize, SpecParams.PageSize);
        }

        public ProductWithBrandAndCategorySpecifications(int id):base(p => p.Id == id)
        {
            AddingBrandsAndCategory();
        }

        private void AddingBrandsAndCategory()
        {
            Includes.Add(p => p.Brands);
            Includes.Add(p => p.Categories);
        }
    }
}
