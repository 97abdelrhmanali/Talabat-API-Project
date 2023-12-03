using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.Core.Service_Contract
{
    public interface IProductServices
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams SpecParams);
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> GetCount(ProductSpecParams SpecParams);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();

    }
}
