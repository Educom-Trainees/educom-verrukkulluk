using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOModels;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICrud _crud;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ICrud crud, IMapper mapper, ILogger<ProductsController> logger)
        {
            _crud = crud;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<ProductDTO> GetProducts()
        {
            IEnumerable<Product> products = _crud.ReadAllProducts();
            IEnumerable<ProductDTO> productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);

            if (productDTOs == null)
            {
                return Enumerable.Empty<ProductDTO>();
            }

            return productDTOs;
        }

        [HttpGet("IngredientTypes")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IngredientType>> GetIngredientTypes()
        {
            return Ok(Enum.GetValues(typeof(IngredientType)).Cast<IngredientType>());
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDTO> GetProduct(int id)
        {
            ProductDTO productDTO = _mapper.Map<ProductDTO>(_crud.ReadProductById(id));

            if (productDTO == null)
            {
                return NotFound();
            }

            return productDTO;
        }
        

        //POST: api/Products
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProductDTO> PostProduct([FromBody]ProductDTO productDto)
        {
            ValidateProductDto(productDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product product = _mapper.Map<Product>(productDto);
            Product createdProduct = _crud.CreateProduct(product);

            if (createdProduct != null)
            {
                // Map the created product back to a DTO
                ProductDTO createdProductDTO = _mapper.Map<ProductDTO>(_crud.ReadProductById(createdProduct.Id));
                // Return the DTO of the created product with the appropriate status code
                return CreatedAtAction("GetProduct", new { id = createdProduct.Id }, createdProductDTO);
            } else
            {
                return Problem("Failed to create product.", statusCode: 500);
            }
        }


        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult PutProduct(int id, ProductDTO productDto)
        {
            ValidateProductDto(productDto, id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                int previousImageObjId = _crud.ReadImageObjIdForProductId(productDto.Id);
                var product = _mapper.Map<Product>(productDto);
                _crud.UpdateProduct(product);
                if (previousImageObjId != product.ImageObjId)
                {
                    _crud.DeletePicture(previousImageObjId);
                }
                return NoContent();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException && _crud.ReadProductById(id) == null)
                {
                    return NotFound();
                }
                _logger.LogError(e, "Update product {ProductDto.Name} failed", productDto.Name);
                return Problem(statusCode: 500);
            }
        }

        private void ValidateProductDto(ProductDTO productDto, int id = 0)
        {
            if (id != productDto.Id)
            {
                ModelState.AddModelError(nameof(ProductDTO.Id), $"The id should be {id}");
            }

            if (!_crud.DoesProductNameAlreadyExist(productDto.Name, productDto.Id)) 
            {
                ModelState.AddModelError(nameof(ProductDTO.Name), "There is another product with this name");
            }

            if (!_crud.DoesPictureExist(productDto.ImageObjId))
            {
                ModelState.AddModelError(nameof(ProductDTO.ImageObjId), "The image is not (yet) stored");
            } else if (!_crud.IsPictureAvailiable(productDto.ImageObjId, EImageObjType.Product, productDto.Id))
            {
                ModelState.AddModelError(nameof(ProductDTO.ImageObjId), "The image is already linked to another object");
            }

            if (!_crud.DoesPackagingTypeExist(productDto.PackagingId))
            {
                ModelState.AddModelError(nameof(ProductDTO.PackagingId), "Unknown packaging id");
            }

            if (!_crud.DoAllergiesExist(productDto.Allergies.Select(a => a.Id).ToArray()))
            {
                ModelState.AddModelError(nameof(ProductDTO.Allergies), "One of the allergy id's is unknown");
            }

        }

        // // DELETE: api/Products/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteProduct(int id)
        // {
        //     var product = await _context.Products.FindAsync(id);
        //     if (product == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Products.Remove(product);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        // private bool ProductExists(int id)
        // {
        //     return _context.Products.Any(e => e.Id == id);
        // }
    }
}
