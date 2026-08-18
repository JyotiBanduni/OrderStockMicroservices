using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Service;


namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(
            IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateProductDto dto)
        {
            var result =
                await _productService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.ProductId },
                result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result =
                await _productService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = 10;
            }

            var result =
                await _productService.GetAllAsync(
                    pageNumber,
                    pageSize);

            return Ok(new
            {
                pageNumber,
                pageSize,
                totalCount = result.TotalCount,
                data = result.Products
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateProductDto dto)
        {
            var result =
                await _productService.UpdateAsync(id, dto);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(new
            {
                message = "Product updated successfully."
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result =
                await _productService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(new
            {
                message = "Product deleted successfully."
            });
        }

        [HttpPatch("{id:guid}/stock")]
        public async Task<IActionResult> ReduceStock(
                Guid id,
                UpdateStockDto dto)
        {
            if (dto.Quantity <= 0)
            {
                return BadRequest(new
                {
                    message = "Quantity must be greater than zero."
                });
            }

            var result =
                await _productService.ReduceStockAsync(
                    id,
                    dto.Quantity);

            if (!result)
            {
                return BadRequest(new
                {
                    message =
                        "Product not found, inactive, or insufficient stock."
                });
            }

            return Ok(new
            {
                message = "Stock updated successfully."
            });
        }
    }
}