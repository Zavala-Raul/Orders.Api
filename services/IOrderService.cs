using Orders.Api.models;
using Orders.Api.models.dtos;

namespace Orders.Api.services
{
    public interface IOrderService
    {
        Task<(bool HasError, string? Message, int? OrderId)> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<(bool HasError, string? Message, List<TransportOrder> Orders)> GetOrdersAsync(string? status, int? customerId, string? orderNumber);
        Task<(bool HasError, string? Message, TransportOrder? Order)> GetOrderByIdAsync(int orderId);
        Task<(bool HasError, bool NotFound, string? Message)> ModifyOrderAsync(int orderId, ModifyOrderDto modifyOrderDto);
        Task<(bool HasError, bool NotFound, string? Message)> FinishOrderAsync(int orderId);
        Task<(bool HasError, bool NotFound, string? Message)> DeleteOrderAsync(int orderId);
    }
}
