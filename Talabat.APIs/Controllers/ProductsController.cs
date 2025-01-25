using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo,
                                  IMapper mapper,
                                  IGenericRepository<ProductType> TypeRepo,
                                  IGenericRepository<ProductBrand> BrandRepo) {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _brandRepo = BrandRepo;
            _typeRepo = TypeRepo;
        }



        //Get all products
        [Authorize]
        [HttpGet]

        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpec(Params);
            var Products = await _productRepo.GetAllWithSpecAsync(Spec);
           
           var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFiltrationForCountAsync(Params);
            var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, MappedProducts , Count));
        } 

        //get products by id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),200)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpec(id);
            var Product = await _productRepo.GetByIdWithSpecAsync(Spec);
            if (Product == null) return NotFound(new ApiResponse(404));
            var MappedProducts = _mapper.Map<Product, ProductToReturnDto>(Product);
            return Ok(MappedProducts);
        }
        //get types
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types= await  _typeRepo.GetAllAsync();
            return Ok(Types);
        }

        //get all brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            return Ok(Brands);
        }




    }
}
