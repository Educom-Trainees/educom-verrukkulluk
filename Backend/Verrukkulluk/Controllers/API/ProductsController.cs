using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Verrukkulluk;
using Verrukkulluk.Data;
using Verrukkulluk.Models;
using Verrukkulluk.Models.DTOModels;
using Verrukkulluk.Models.ViewModels;

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

        /// <summary>
        /// Get a list of all products including the allergy info
        /// </summary>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "list of all products", typeof(List<ProductDTO>))]
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
        /// <summary>
        /// Get All ingredient types
        /// </summary>
        /// <returns></returns>
        [HttpGet("IngredientTypes")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<IngredientType>> GetIngredientTypes()
        {
            return Enum.GetValues<IngredientType>();
        }
        /// <summary>
        /// Get a specific product
        /// </summary>
        /// <param name="id">the product id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "The product with this id", typeof(ProductDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When not found")]
        public ActionResult<ProductDTO> GetProduct(int id)
        {
            ProductDTO productDTO = _mapper.Map<ProductDTO>(_crud.ReadProductById(id));

            if (productDTO == null)
            {
                return NotFound();
            }

            return productDTO;
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="productDto">The product</param>
        /// <returns>the created product</returns>
        /// <remarks>
        /// Save the image first using a POST on /api/ImageObj, use the resulting number (for example 56) as image object id
        /// 
        /// Sample request
        ///  
        ///     POST /Products
        ///     {
        ///       "name": "Spagaroni",
        ///       "description": "Verpakking spaghetti macaroni (500 g)",
        ///       "price": 2.55,
        ///       "calories": 1835,
        ///       "amount": 500,
        ///       "imageObjId": 56,
        ///       "smallestAmount": 1,
        ///       "packagingId": 14,
        ///       "ingredientType": "gram",
        ///       "active": true,
        ///       "allergies": [
        ///         {
        ///           "id": 2
        ///         },
        ///         {
        ///           "id": 5
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status201Created, "The product with this id", typeof(ProductDTO))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "On error or duplicate name", typeof(ErrorExample))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "When a product could not be created")]
        public ActionResult<ProductDTO> PostProduct([FromBody] ProductDTO productDto)
        {
            ValidateProductDto(productDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _crud.CreateProduct(product);

                // Map the created product back to a DTO
                ProductDTO createdProductDTO = _mapper.Map<ProductDTO>(_crud.ReadProductById(product.Id));
                // Return the DTO of the created product with the appropriate status code
                return CreatedAtAction("GetProduct", new { id = createdProductDTO.Id }, createdProductDTO);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Create product {ProductDto.Name} failed", productDto.Name);
                return Problem("Failed to create product.", statusCode: 500);
            }
        }


        /// <summary>
        /// Change a product
        /// </summary>
        /// <param name="id">the product id</param>
        /// <param name="productDto">the product</param>
        /// <remarks>
        /// (optionally) save a new image first using a POST on /api/ImageObj, use the resulting number (for example 56) as image object id
        /// 
        /// Sample request
        ///  
        ///     PUT /Products/8
        ///     {
        ///       "id": 8,
        ///       "name": "Spaghetti",
        ///       "description": "Verpakking (500 g)",
        ///       "price": 2.55,
        ///       "calories": 1835,
        ///       "amount": 500,
        ///       "imageObjId": 13,
        ///       "smallestAmount": 1,
        ///       "packagingId": 14,
        ///       "ingredientType": "gram",
        ///       "active": true,
        ///       "allergies": [
        ///         {
        ///           "id": 2
        ///         }
        ///       ]
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "When succeeded")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "On error or duplicate name", typeof(ErrorExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When product not found")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "When a product could not be modified")]
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

            if (_crud.DoesProductNameAlreadyExist(productDto.Name, productDto.Id))
            {
                ModelState.AddModelError(nameof(ProductDTO.Name), "There is another product with this name");
            }

            if (!_crud.DoesPictureExist(productDto.ImageObjId))
            {
                ModelState.AddModelError(nameof(ProductDTO.ImageObjId), "The image is not (yet) stored");
            } else if (!_crud.IsPictureAvailable(productDto.ImageObjId, EImageObjType.Product, productDto.Id))
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

        /// <summary>
        /// Toggle product active
        /// </summary>
        /// <param name="id">The productId</param>
        /// <param name="active">(optional) make product (in)active, when absent the active flag is toggled</param>
        /// <remarks>
        /// Sample request
        ///  
        ///     PATCH /Products/4/active                   - toggles the active flag 
        ///     
        ///     PATCH /products/4/active?active=false      - disables the active flag
        /// 
        /// </remarks>
        [HttpPatch("{id}/active")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "When succeeded")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When product not found")]
        public IActionResult ToggleActive(int id, [FromQuery]bool? active)
        {
            var product = _crud.ReadProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Active = active ?? !product.Active;
            _crud.UpdateProduct(product);

            return NoContent();
        }

        /// <summary>
        /// Delete an unused product
        /// </summary>
        /// <param name="id">the id of the product</param>
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "When succeeded")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "When product not found")]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "When product still in use")]
        public IActionResult DeleteProduct(int id)
        {
            if (_crud.IsProductUsed(id))
            {
                return Problem("product is still in use, try to deactivate it", statusCode: 422);
            }

            if (!_crud.DeleteProduct(id))
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
