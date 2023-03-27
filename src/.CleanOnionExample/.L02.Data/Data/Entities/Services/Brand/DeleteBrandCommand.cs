namespace CleanOnionExample.Data.Entities.Services;
public class DeleteBrandCommand : IRequest<Result<int>> {
  public int Id { get; set; }

  public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result<int>> {
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork) {
      _brandRepository = brandRepository;
      _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteBrandCommand command, CancellationToken cancellationToken) {
      var product = await _brandRepository.GetByIdAsync(command.Id);
      await _brandRepository.DeleteAsync(product);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
      return Result<int>.Success(product.Id);
    }
  }
}

public partial class CreateBrand {
  public partial class Command : IRequest<Result<int>> {
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }
  }

  public class CommandHandler : IRequestHandler<Command, Result<int>> {
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    private IUnitOfWork _unitOfWork { get; set; }

    public CommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork, IMapper mapper) {
      _brandRepository = brandRepository;
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken) {
      var product = _mapper.Map<Brand>(request);
      await _brandRepository.InsertAsync(product);
      await _unitOfWork.SaveChangesAsync(cancellationToken);
      return Result<int>.Success(product.Id);
    }
  }
}

public class UpdateBrand {
  public class Command : IRequest<Result<int>> {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }

    public class CommandHandler : IRequestHandler<Command, Result<int>> {
      private readonly IUnitOfWork _unitOfWork;
      private readonly IBrandRepository _brandRepository;

      public CommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork) {
        _brandRepository = brandRepository;
        _unitOfWork = unitOfWork;
      }

      public async Task<Result<int>> Handle(Command command, CancellationToken cancellationToken) {
        var brand = await _brandRepository.GetByIdAsync(command.Id);

        if (brand == null) {
          return Result<int>.Fail($"Brand Not Found.");
        } else {
          brand.Name = command.Name ?? brand.Name;
          brand.Tax = (command.Tax == 0) ? brand.Tax : command.Tax;
          brand.Description = command.Description ?? brand.Description;
          await _brandRepository.UpdateAsync(brand);
          await _unitOfWork.SaveChangesAsync(cancellationToken);
          return Result<int>.Success(brand.Id);
        }
      }
    }
  }
}

public class GetAllBrandsCached {
  public class Response {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }
  }

  public class Query : IRequest<Result<List<Response>>> {
    public Query() {
    }
  }

  public class GetAllBrandsCachedQueryHandler : IRequestHandler<Query, Result<List<Response>>> {
    private readonly IBrandCacheRepository _productCache;
    private readonly IMapper _mapper;

    public GetAllBrandsCachedQueryHandler(IBrandCacheRepository productCache, IMapper mapper) {
      _productCache = productCache;
      _mapper = mapper;
    }

    public async Task<Result<List<Response>>> Handle(Query request, CancellationToken cancellationToken) {
      var brandList = await _productCache.GetCachedListAsync();
      var mappedBrands = _mapper.Map<List<Response>>(brandList);
      return Result<List<Response>>.Success(mappedBrands);
    }
  }
}

public class GetBrandById {
  public class Query : IRequest<Result<Response>> {
    public int Id { get; set; }

    public class GetProductByIdQueryHandler : IRequestHandler<Query, Result<Response>> {
      private readonly IBrandCacheRepository _brandCache;
      private readonly IMapper _mapper;

      public GetProductByIdQueryHandler(IBrandCacheRepository productCache, IMapper mapper) {
        _brandCache = productCache;
        _mapper = mapper;
      }

      public async Task<Result<Response>> Handle(Query query, CancellationToken cancellationToken) {
        var product = await _brandCache.GetByIdAsync(query.Id);
        var mappedProduct = _mapper.Map<Response>(product);
        return Result<Response>.Success(mappedProduct);
      }
    }
  }

  public class Response {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Tax { get; set; }
    public string Description { get; set; }
  }

}