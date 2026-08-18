using Microsoft.AspNetCore.Mvc;
using OrderService.DTOs;
using OrderService.Services;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateOrderDto dto)
        {
            var result =
                await _orderService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.OrderId },
                result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id)
        {
            var result =
                await _orderService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
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
                await _orderService.GetAllAsync(
                    pageNumber,
                    pageSize);

            return Ok(new
            {
                pageNumber,
                pageSize,
                totalCount = result.TotalCount,
                data = result.Orders
            });
        }
    }
}