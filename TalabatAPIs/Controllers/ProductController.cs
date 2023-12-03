using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Service_Contract;
using Talabat.Core.Specifications.ProductSpec;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;
using TalabatAPIs.Helper;

namespace TalabatAPIs.Controllers
{
    public class ProductController : ApiBaseController
    {
        ///private readonly IGenericRepository<Product> _productRepository;
        ///private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        ///private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
        
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;

        public ProductController(
            ///IGenericRepository<Product> productRepository,
            ///IGenericRepository<ProductBrand> productBrandRepository,
            ///IGenericRepository<ProductCategory> productCategoryRepository,
            
            IProductServices productServices,
            IMapper mapper)
        {
            ///_productRepository = productRepository;
            ///_productBrandRepository = productBrandRepository;
            ///_productCategoryRepository = productCategoryRepository;
           
            _productServices = productServices;
            _mapper = mapper;
        }

        [CashedAttribute(600)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAll([FromQuery]ProductSpecParams SpecParams)
        { 
            var products = await _productServices.GetProductsAsync( SpecParams );
            var count = await _productServices.GetCount(SpecParams);
            //var c = products.Count;
            var productDto = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(SpecParams.PageIndex,SpecParams.PageSize,count,productDto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRespone),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var Product = await _productServices.GetProductByIdAsync(id);
            if (Product is null)
                return NotFound(new ApiRespone(404));
            var ProductDto = _mapper.Map<Product , ProductToReturnDto>(Product);
            return Ok(ProductDto);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrand()
        {
            var brand = await _productServices.GetBrandsAsync();
            return Ok(brand);
        }

        [HttpGet("categories")]    
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetProductCategory()
        {
            var categories = await _productServices.GetCategoriesAsync();
            return Ok(categories);
        }

    }
}
