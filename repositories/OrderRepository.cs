using System.Data;
using Dapper;
using Orders.Api.data;
using Orders.Api.models;
using Orders.Api.models.dtos;

namespace Orders.Api.repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDapperService _dapperService;
        private const string ConnectionId = "DefaultConnection";
        private const string Schema = "dbo";

        public OrderRepository(IDapperService dapperService)
        {
            _dapperService = dapperService;
        }

        private static StoredProcedureInf Sp(string name) => new()
        {
            IdConnectionString = ConnectionId,
            SchemaName = Schema,
            Name = name
        };

        public async Task<DapperResponse> CreateAsync(CreateOrderDto dto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderNumber", dto.OrderNumber);
            parameters.Add("@CustomerId", dto.CustomerId);
            parameters.Add("@VehicleId", dto.VehicleId);
            parameters.Add("@OriginCity", dto.OriginCity);
            parameters.Add("@DestinationCity", dto.DestinationCity);
            parameters.Add("@CargoDescription", dto.CargoDescription);
            parameters.Add("@WeightKg", dto.WeightKg);
            parameters.Add("@FreightAmount", dto.FreightAmount);
            parameters.Add("@CurrencyCode", dto.CurrencyCode);
            parameters.Add("@Status", dto.Status);
            parameters.Add("@PickupDate", dto.PickupDate, DbType.Date);
            parameters.Add("@DeliveryDate", dto.DeliveryDate, DbType.Date);
            parameters.Add("@Notes", dto.Notes);
            parameters.Add("@OrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var response = await _dapperService.ExecuteStoredProcedureAsync<int>(Sp("usp_TransportOrders_Create"), parameters);

            if (response.Success)
            {
                response.Result = parameters.Get<int>("@OrderId");
            }
            return response;
        }

        public async Task<DapperResponse> GetByIdAsync(int orderId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId);

            return await _dapperService.ExecuteStoredProcedureAsync<TransportOrder>(
                Sp("usp_TransportOrders_GetById"), parameters, hasArray: false);
        }

        public async Task<DapperResponse> GetAllAsync(string? status, int? customerId, string? orderNumber)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Status", status);
            parameters.Add("@CustomerId", customerId);
            parameters.Add("@OrderNumber", orderNumber);

            return await _dapperService.ExecuteStoredProcedureAsync<TransportOrder>(
                Sp("usp_TransportOrders_GetAll"), parameters, hasArray: true);
        }

        public async Task<DapperResponse> UpdateAsync(TransportOrder order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", order.OrderId);
            parameters.Add("@OrderNumber", order.OrderNumber);
            parameters.Add("@CustomerId", order.CustomerId);
            parameters.Add("@VehicleId", order.VehicleId);
            parameters.Add("@OriginCity", order.OriginCity);
            parameters.Add("@DestinationCity", order.DestinationCity);
            parameters.Add("@CargoDescription", order.CargoDescription);
            parameters.Add("@WeightKg", order.WeightKg);
            parameters.Add("@FreightAmount", order.FreightAmount);
            parameters.Add("@CurrencyCode", order.CurrencyCode);
            parameters.Add("@Status", order.Status);
            parameters.Add("@PickupDate", order.PickupDate, DbType.Date);
            parameters.Add("@DeliveryDate", order.DeliveryDate, DbType.Date);
            parameters.Add("@Notes", order.Notes);

            return await _dapperService.ExecuteStoredProcedureAsync<int>(
                Sp("usp_TransportOrders_Update"), parameters);
        }

        public async Task<DapperResponse> DeleteAsync(int orderId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId);

            return await _dapperService.ExecuteStoredProcedureAsync<int>(
                Sp("usp_TransportOrders_Delete"), parameters);
        }
    }
}
