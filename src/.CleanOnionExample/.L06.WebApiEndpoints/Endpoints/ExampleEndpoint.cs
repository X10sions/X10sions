using FastEndpoints;

namespace CleanOnionExample.Endpoints;
public class ExampleEndpoint : EndpointWithoutRequest {

  public override void Configure() {
    Verbs(Http.GET);
    Routes("example");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct) {
    await SendAsync(new {
      message = "Hellow world"
    }, cancellation: ct);
  }

}