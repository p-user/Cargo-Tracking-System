﻿namespace Order.Api.Configurations
{
    public class Automapper : Profile
    {

        public Automapper()
        {
            CreateMap<DeliveryOrder, ViewDeliveryOrderDto>()
                .ForMember(dest => dest.DeliveryAddress, opt => opt.MapFrom(src => src.DeliveryAddress)).ReverseMap();

            CreateMap<DeliveryOrder, CreateDeliveryOrderDto>()
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Cargo))
                .ReverseMap();

            CreateMap<CargoDetails, CargoDetailsDto>()
                .ReverseMap();


            CreateMap<Customer, CustomerDto>()
                .ReverseMap();


            CreateMap<Customer, CreateCustomerDto>()
               .ReverseMap();

            CreateMap<Customer, ViewCustomerDto>()
               .ReverseMap();

            CreateMap<Address, AddressDto>()
                .ReverseMap();

        }
    }
}
