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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDTO> PostProduct(ProductDTO productDTO)
        {
            Product product = _mapper.Map<Product>(productDTO);

            Product createdProduct = _crud.CreateProduct(product);

            if (createdProduct != null)
            {
                // Map the created product back to a DTO
                ProductDTO createdProductDTO = _mapper.Map<ProductDTO>(createdProduct);
                // Return the DTO of the created product with the appropriate status code
                return CreatedAtAction("GetProduct", new { id = createdProduct.Id }, createdProductDTO);
            } else
            {
                return BadRequest("Failed to create product.");
            }
        }


        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutProduct(int id, ProductDTO productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("Id does not match");
            }

            Product? product = null;
            if (ModelState.IsValid(nameof(ProductDTO.Name)))
            {
                product = _crud.ReadProductByName(productDto.Name);
                if (product != null && product.Id != id)
                {
                    ModelState.AddModelError(nameof(ProductDTO.Name), "Een ander product heeft deze naam al");
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // if we haven't found the product by name yet, search by id
            product ??= _crud.ReadProductById(id);
            if (product == null)
            {
               return NotFound();
            }
            _logger.LogError("Update product {ProductDto.Name} failed", productDto.Name);
            try
            {
                _mapper.Map(productDto, product);
                _crud.UpdateProduct(product);
                return NoContent();                
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update product {ProductDto.Name} failed", productDto.Name);
                return Problem(statusCode: 500);
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
