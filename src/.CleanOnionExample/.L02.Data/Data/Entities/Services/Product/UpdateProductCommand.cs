using System.Linq.Expressions;

namespace CleanOnionExample.Data.Entities.Services;


public class GetAllProductsQuery : IRequest<PaginatedResult<GetAllProductsResponse>> {
  public int PageNumber { get; set; }
  public int PageSize { get; set; }

  public GetAllProductsQuery(int pageNumber, int pageSize) {
    PageNumber = pageNumber;
    PageSize = pageSize;
  }
}

public class GetProductByIdResponse {
  public int Id { get; set; }
  public string Name { get; set; }
  public string Barcode { get; set; }
  public byte[] Image { get; set; }
  public string Description { get; set; }
  public decimal Rate { get; set; }
  public int BrandId { get; set; }
}

public class GetProductByIdQuery : IRequest<Result<GetProductByIdResponse>> {
  public int Id { get; set; }

  public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdResponse>> {
    private readonly IProductCacheRepository _productCache;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductCacheRepository productCache, IMapper mapper) {
      _productCache = productCache;
      _mapper = mapper;
    }

    public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken) {
      var product = await _productCache.GetByIdAsync(query.Id);
      var mappedProduct = _mapper.Map<GetProductByIdResponse>(product);
      return Result<GetProductByIdResponse>.Success(mappedProduct);
    }
  }
}

public class GetAllProductsResponse {
  public int Id { get; set; }
  public string Name { get; set; }
  public string Barcode { get; set; }
  public string Description { get; set; }
  public decimal Rate { get; set; }
}

public class GGetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedResult<GetAllProductsResponse>> {
  private readonly IProductRepository _repository;

  public GGetAllProductsQueryHandler(IProductRepository repository) {
    _repository = repository;
  }

  public async Task<PaginatedResult<GetAllProductsResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken) {
    Expression<Func<Product, GetAllProductsResponse>> expression = e => new GetAllProductsResponse {
      Id = e.Id,
      Name = e.Name,
      Description = e.Description,
      Rate = e.Rate,
      Barcode = e.Barcode
    };
    var paginatedList = await _repository.Products.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
    return paginatedList;
  }
}


public class GetAllProductsCachedResponse {
  public int Id { get; set; }
  public string Name { get; set; }
  public string Barcode { get; set; }
  public byte[] Image { get; set; }
  public string Description { get; set; }
  public decimal Rate { get; set; }
  public int BrandId { get; set; }
}

public class GetAllProductsCachedQuery : IRequest<Result<List<GetAllProductsCachedResponse>>> {
  public GetAllProductsCachedQuery() {
  }
}

public class GetAllProductsCachedQueryHandler : IRequestHandler<GetAllProductsCachedQuery, Result<List<GetAllProductsCachedResponse>>> {
  private readonly IProductCacheRepository _productCache;
  private readonly IMapper _mapper;

  public GetAllProductsCachedQueryHandler(IProductCacheRepository productCache, IMapper mapper) {
    _productCache = productCache;
    _mapper = mapper;
  }

  public async Task<Result<List<GetAllProductsCachedResponse>>> Handle(GetAllProductsCachedQuery request, CancellationToken cancellationToken) {
    var productList = await _productCache.GetCachedListAsync();
    var mappedProducts = _mapper.Map<List<GetAllProductsCachedResponse>>(productList);
    return Result<List<GetAllProductsCachedResponse>>.Success(mappedProducts);
  }
}

public partial class CreateProductCommand : IRequest<Result<int>> {
  public string Name { get; set; }
  public string Barcode { get; set; }
  public string Description { get; set; }
  public decimal Rate { get; set; }
  public int BrandId { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<int>> {
  private readonly IProductRepository _productRepository;
  private readonly IMapper _mapper;

  private IUnitOfWork _unitOfWork { get; set; }

  public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper) {
    _productRepository = productRepository;
    _unitOfWork = unitOfWork;
    _mapper = mapper;
  }

  public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken) {
    var product = _mapper.Map<Product>(request);
    await _productRepository.InsertAsync(product);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
    return Result<int>.Success(product.Id);
  }
}

public class DeleteProductCommand : IRequest<Result<int>> {
  public int Id { get; set; }

  public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<int>> {
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) {
      _productRepository = productRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteProductCommand command, CancellationToken cancellationToken) {
      var product = await _productRepository.GetByIdAsync(command.Id);
      await _productRepository.DeleteAsync(product);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
      return Result<int>.Success(product.Id);
    }
  }
}
public class UpdateProductCommand : IRequest<Result<int>> {
  public int Id { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public decimal Rate { get; set; }
  public int BrandId { get; set; }

  public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<int>> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) {
      _productRepository = productRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(UpdateProductCommand command, CancellationToken cancellationToken) {
      var product = await _productRepository.GetByIdAsync(command.Id);

      if (product == null) {
        return Result<int>.Fail($"Product Not Found.");
      } else {
        product.Name = command.Name ?? product.Name;
        product.Rate = (command.Rate == 0) ? product.Rate : command.Rate;
        product.Description = command.Description ?? product.Description;
        product.BrandId = (command.BrandId == 0) ? product.BrandId : command.BrandId;
        await _productRepository.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(product.Id);
      }
    }
  }
}

public class UpdateProductImageCommand : IRequest<Result<int>> {
  public int Id { get; set; }
  public byte[] Image { get; set; }

  public class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommand, Result<int>> {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public UpdateProductImageCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork) {
      _productRepository = productRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(UpdateProductImageCommand command, CancellationToken cancellationToken) {
      var product = await _productRepository.GetByIdAsync(command.Id);

      if (product == null) {
        throw new ApiException($"Product Not Found.");
      } else {
        product.Image = command.Image;
        await _productRepository.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(product.Id);
      }
    }
  }
}
