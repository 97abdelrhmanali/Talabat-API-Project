using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Service_Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpec;

namespace Talabat.Service
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams SpecParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(SpecParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            return products;
        }
        public async Task<int> GetCount(ProductSpecParams SpecParams)
        {
            var CountSpec = new ProductWithFilterationAndCountSpecification(SpecParams);
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(CountSpec);
            return count;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (Product is null) return null;
            return Product;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();


        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
            => await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
    }
}
