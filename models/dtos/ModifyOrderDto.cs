using System.ComponentModel.DataAnnotations;

namespace Orders.Api.models.dtos
{
    public sealed class ModifyOrderDto
    {
        [StringLength(20)]
        public string? OrderNumber { get; set; }
        public int? CustomerId { get; set; }
        public int? VehicleId { get; set; }
        [StringLength(80)]
        public string? OriginCity { get; set; }
        [StringLength(80)]
        public string? DestinationCity { get; set; }
        [StringLength(200)]
        public string? CargoDescription { get; set; }
        public decimal? WeightKg { get; set; }
        public decimal? FreightAmount { get; set; }
        [RegularExpression("MXN|USD")]
        public string? CurrencyCode { get; set; }
        [RegularExpression("Created|Assigned|InTransit|Delivered|Cancelled")]
        public string? Status { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
