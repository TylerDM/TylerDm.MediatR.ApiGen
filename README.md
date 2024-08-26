# TylerDM.MediatR.Apigen
A source generator to automatically create API controllers from your MediatR handlers.

## Usage
Just place a standard ASP.Net Core MVC [Route] attribute on your handler and the generator will create the controller for you at compile time.
```csharp
[Route("/TestingRoute")]
public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
{
	public Task<TestResponse> Handle(TestRequest request, CancellationToken cancellationToken) =>
		Task.FromResult(new TestResponse($"Hello {request.Name}!"));
}
```

[Nuget](https://www.nuget.org/packages/TylerDM.MediatR.Apigen)