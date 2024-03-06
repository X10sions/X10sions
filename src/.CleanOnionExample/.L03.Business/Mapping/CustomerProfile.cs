using AutoMapper;
using CleanOnionExample.Data.Entities;
using CleanOnionExample.Data.Entities.Services;
namespace CleanOnionExample.Infrastructure.Mapping;


public class CustomerProfile : Profile {
  public CustomerProfile() {
    CreateMap<CustomerModel, Customer>()
        .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.CustomerId))
        .ReverseMap();
  }
}

internal class BrandProfile : Profile {
  public BrandProfile() {
    CreateMap<CreateBrand.Command, Brand>().ReverseMap();
    CreateMap<GetBrandById.Response, Brand>().ReverseMap();
    CreateMap<GetAllBrandsCached.Response, Brand>().ReverseMap();
  }
}

internal class ProductProfile : Profile {
  public ProductProfile() {
    CreateMap<CreateProductCommand, Product>().ReverseMap();
    CreateMap<GetProductByIdResponse, Product>().ReverseMap();
    CreateMap<GetAllProductsCachedResponse, Product>().ReverseMap();
    CreateMap<GetAllProductsResponse, Product>().ReverseMap();
  }
}