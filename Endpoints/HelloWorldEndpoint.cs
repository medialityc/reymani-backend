using System;

using FastEndpoints;

namespace reymani_web_api.Endpoints;

public class HelloWorldEndpoint : EndpointWithoutRequest
{
  public override void Configure()
  {
    Verbs(Http.GET);
    Routes("/hello-world");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await SendAsync("Hello, World!", cancellation: ct);
  }
}