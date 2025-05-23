﻿using Order.Api.Enums;

namespace Order.Api.Dtos
{
    public record DeliveryOrderDto
    {
       
        public string ReceiverName { get; set; }
        public string ReceiverContact { get; set; }

        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }

        public CargoDetailsDto Cargo { get; set; }

        public DeliveryStatus Status { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public Guid? TrackingId { get; set; }
    }


    public record CreateDeliveryOrderDto : DeliveryOrderDto
    {
        public CustomerDto Customer { get; set; } 

    }

    public record ViewDeliveryOrderDto : DeliveryOrderDto
    {
        public Guid Id { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? LastModified { get; init; }
        public string? CreatedBy { get; init; }
        public string? LastModifiedBy { get; init; }
        public ViewCustomerDto ViewCustomer { get; init; }


    }
}
