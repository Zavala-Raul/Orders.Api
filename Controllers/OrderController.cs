using Microsoft.AspNetCore.Mvc;
using Orders.Api.models.dtos;
using Orders.Api.services;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
    {
        var (hasError, message, orderId) = await _orderService.CreateOrderAsync(createOrderDto);
        return hasError
            ? BadRequest(new { Message = message })
            : StatusCode(201, new { OrderId = orderId });
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] string? status, [FromQuery] int? customerId, [FromQuery] string? orderNumber)
    {
        var (hasError, message, orders) = await _orderService.GetOrdersAsync(status, customerId, orderNumber);
        return hasError ? StatusCode(500, new { Message = message }) : Ok(orders);
    }

    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetOrderById(int orderId)
    {
        var (hasError, message, order) = await _orderService.GetOrderByIdAsync(orderId);
        if (hasError) return StatusCode(500, new { Message = message });
        return order is null ? NotFound(new { Message = $"No existe la orden {orderId}." }) : Ok(order);
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetTotalOrders()
    {
        var (hasError, message, orders) = await _orderService.GetOrdersAsync(null, null, null);
        return hasError ? StatusCode(500, new { Message = message }) : Ok(new { totalOrders = orders.Count });
    }

    [HttpGet("terminadas")]
    public async Task<IActionResult> GetOrdenesTerminadas()
    {
        var (hasError, message, orders) = await _orderService.GetOrdersAsync("Delivered", null, null);
        return hasError ? StatusCode(500, new { Message = message }) : Ok(new { finishedOrders = orders });
    }

    [HttpPut("{orderId:int}")]
    public async Task<IActionResult> ModifyOrder(int orderId, ModifyOrderDto modifyOrderDto)
    {
        var (hasError, notFound, message) = await _orderService.ModifyOrderAsync(orderId, modifyOrderDto);
        if (notFound) return NotFound(new { Message = $"No existe la orden {orderId}." });
        return hasError ? BadRequest(new { Message = message }) : NoContent();
    }

    [HttpPut("{orderId:int}/terminar")]
    public async Task<IActionResult> TerminarOrden(int orderId)
    {
        var (hasError, notFound, message) = await _orderService.FinishOrderAsync(orderId);
        if (notFound) return NotFound(new { Message = $"No existe la orden {orderId}." });
        return hasError ? BadRequest(new { Message = message }) : NoContent();
    }

    [HttpDelete("{orderId:int}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var (hasError, notFound, message) = await _orderService.DeleteOrderAsync(orderId);
        if (notFound) return NotFound(new { Message = $"No existe la orden {orderId}." });
        return hasError ? BadRequest(new { Message = message }) : NoContent();
    }
}
