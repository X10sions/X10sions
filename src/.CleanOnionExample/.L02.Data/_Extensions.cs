using Common.Behaviours;
using Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanOnionExample;

public static class Extensions {
  public static IServiceCollection AddAppCoreCleanOnionExample(this IServiceCollection services) {
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddMediatR(Assembly.GetExecutingAssembly());
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

    return services;
  }
}

public static class QueryableExtensions {
  public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class {
    if (source is null) throw new ArgumentNullException(nameof(source));
    pageNumber = pageNumber == 0 ? 1 : pageNumber;
    pageSize = pageSize == 0 ? 10 : pageSize;
    var count = await source.LongCountAsync();
    pageNumber = pageNumber <= 0 ? 1 : pageNumber;
    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
  }
}