using Orders.Api.models;
using Orders.Api.models.dtos;

namespace Orders.Api.repositories
{
    public interface IOrderRepository
    {
        Task<DapperResponse> CreateAsync(CreateOrderDto dto);
        Task<DapperResponse> GetByIdAsync(int orderId);
        Task<DapperResponse> GetAllAsync(string? status, int? customerId, string? orderNumber);
        Task<DapperResponse> UpdateAsync(TransportOrder order);
        Task<DapperResponse> DeleteAsync(int orderId);
    }
}
