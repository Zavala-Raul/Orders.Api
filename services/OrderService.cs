using Orders.Api.models;
using Orders.Api.models.dtos;
using Orders.Api.repositories;

namespace Orders.Api.services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<(bool HasError, string? Message, int? OrderId)> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var response = await _orderRepository.CreateAsync(createOrderDto);
            if (!response.Success)
            {
                return (true, response.InnerException, null);
            }
            return (false, null, (int)response.Result!);
        }

        public async Task<(bool HasError, string? Message, List<TransportOrder> Orders)> GetOrdersAsync(string? status, int? customerId, string? orderNumber)
        {
            var response = await _orderRepository.GetAllAsync(status, customerId, orderNumber);
            if (!response.Success || response.Result is not IEnumerable<TransportOrder> orders)
            {
                return (true, response.InnerException, []);
            }
            return (false, null, orders.ToList());
        }

        public async Task<(bool HasError, string? Message, TransportOrder? Order)> GetOrderByIdAsync(int orderId)
        {
            var response = await _orderRepository.GetByIdAsync(orderId);
            if (!response.Success)
            {
                return (true, response.InnerException, null);
            }
            return (false, null, response.Result as TransportOrder);
        }

        public async Task<(bool HasError, bool NotFound, string? Message)> ModifyOrderAsync(int orderId, ModifyOrderDto dto)
        {
            var (hasError, message, current) = await GetOrderByIdAsync(orderId);
            if (hasError) return (true, false, message);
            if (current is null) return (false, true, null);

            current.OrderNumber = dto.OrderNumber ?? current.OrderNumber;
            current.CustomerId = dto.CustomerId ?? current.CustomerId;
            current.VehicleId = dto.VehicleId ?? current.VehicleId;
            current.OriginCity = dto.OriginCity ?? current.OriginCity;
            current.DestinationCity = dto.DestinationCity ?? current.DestinationCity;
            current.CargoDescription = dto.CargoDescription ?? current.CargoDescription;
            current.WeightKg = dto.WeightKg ?? current.WeightKg;
            current.FreightAmount = dto.FreightAmount ?? current.FreightAmount;
            current.CurrencyCode = dto.CurrencyCode ?? current.CurrencyCode;
            current.Status = dto.Status ?? current.Status;
            current.PickupDate = dto.PickupDate ?? current.PickupDate;
            current.DeliveryDate = dto.DeliveryDate ?? current.DeliveryDate;
            current.Notes = dto.Notes ?? current.Notes;

            var response = await _orderRepository.UpdateAsync(current);
            return response.Success ? (false, false, null) : (true, false, response.InnerException);
        }

        public Task<(bool HasError, bool NotFound, string? Message)> FinishOrderAsync(int orderId)
        {
            return ModifyOrderAsync(orderId, new ModifyOrderDto
            {
                Status = "Delivered",
                DeliveryDate = DateTime.Today
            });
        }

        public async Task<(bool HasError, bool NotFound, string? Message)> DeleteOrderAsync(int orderId)
        {
            var (hasError, message, current) = await GetOrderByIdAsync(orderId);
            if (hasError) return (true, false, message);
            if (current is null) return (false, true, null);

            var response = await _orderRepository.DeleteAsync(orderId);
            return response.Success ? (false, false, null) : (true, false, response.InnerException);
        }
    }
}
