using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using X10sions.Wsus.Data;
using X10sions.Wsus.Data.Models.Susdb;

namespace X10sions.Wsus.Api.Endpoints;

public static class ComputerTargetEndpoints {

  record KeyVale(int Id, string? Name);

  public static void MapComputerTargetEndpoints(this IEndpointRouteBuilder routes) {
    var group = routes.MapGroup("/api/ComputerTarget").WithTags(nameof(ComputerTarget));

    group.MapGet("/list", async (SusdbDbContext db) => {
      return await db.ComputerTarget.Select(x => new KeyVale(x.TargetID, x.FullDomainName)).ToListAsync();
    }).WithName("ListAllComputerTargets").WithOpenApi();


    group.MapGet("/", async (SusdbDbContext db) => {
      return await db.ComputerTarget.ToListAsync();
    }).WithName("GetAllComputerTargets").WithOpenApi();

    group.MapGet("/{id}", async Task<Results<Ok<ComputerTarget>, NotFound>> (int targetid, SusdbDbContext db) => {
      return await db.ComputerTarget.AsNoTracking()
          .FirstOrDefaultAsync(model => model.TargetID == targetid)
          is ComputerTarget model ? TypedResults.Ok(model) : TypedResults.NotFound();
    }).WithName("GetComputerTargetById").WithOpenApi();

    group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int targetid, ComputerTarget computerTarget, SusdbDbContext db) => {
      var affected = await db.ComputerTarget
          .Where(model => model.TargetID == targetid)
          .ExecuteUpdateAsync(setters => setters
              .SetProperty(m => m.TargetID, computerTarget.TargetID)
              .SetProperty(m => m.ComputerID, computerTarget.ComputerID)
              .SetProperty(m => m.SID, computerTarget.SID)
              .SetProperty(m => m.LastSyncTime, computerTarget.LastSyncTime)
              .SetProperty(m => m.LastReportedStatusTime, computerTarget.LastReportedStatusTime)
              .SetProperty(m => m.LastReportedRebootTime, computerTarget.LastReportedRebootTime)
              .SetProperty(m => m.IPAddress, computerTarget.IPAddress)
              .SetProperty(m => m.FullDomainName, computerTarget.FullDomainName)
              .SetProperty(m => m.IsRegistered, computerTarget.IsRegistered)
              .SetProperty(m => m.LastInventoryTime, computerTarget.LastInventoryTime)
              .SetProperty(m => m.LastNameChangeTime, computerTarget.LastNameChangeTime)
              .SetProperty(m => m.EffectiveLastDetectionTime, computerTarget.EffectiveLastDetectionTime)
              .SetProperty(m => m.ParentServerTargetID, computerTarget.ParentServerTargetID)
              .SetProperty(m => m.LastSyncResult, computerTarget.LastSyncResult)
              );
      return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
    })
    .WithName("UpdateComputerTarget").WithOpenApi();

    group.MapPost("/", async (ComputerTarget computerTarget, SusdbDbContext db) => {
      db.ComputerTarget.Add(computerTarget);
      await db.SaveChangesAsync();
      return TypedResults.Created($"/api/ComputerTarget/{computerTarget.TargetID}", computerTarget);
    }).WithName("CreateComputerTarget").WithOpenApi();

    group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int targetid, SusdbDbContext db) => {
      var affected = await db.ComputerTarget
          .Where(model => model.TargetID == targetid)
          .ExecuteDeleteAsync();
      return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
    }).WithName("DeleteComputerTarget").WithOpenApi();
  }
}
