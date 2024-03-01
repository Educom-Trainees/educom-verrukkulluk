using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Verrukkulluk;
using Verrukkulluk.Data;

namespace Verrukkulluk.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ICrud _crud;

        public ProductsController(ICrud crud)
        {
            _crud = crud;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return _crud.ReadAllProducts();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _crud.ReadProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        //POST: api/Products
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            Product createdProduct = _crud.CreateProduct(product);

            if (createdProduct != null)
            {
                return CreatedAtAction("GetProduct", new { id = createdProduct.Id }, createdProduct);
            } else
            {
                return BadRequest("Failed to create product.");
            }
        }


        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutProduct(int id, Product product)
        // {
        //     if (id != product.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(product).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ProductExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

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
