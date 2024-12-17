using System;

namespace reymani_web_api.Api.Endpoints;

public sealed class HelloWorldEndpoint : EndpointWithoutRequest
{
  public override void Configure()
  {
    Get("/hola");
    Permissions("Eliminar_Cliente");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await SendOkAsync("Hello World!");
  }
}