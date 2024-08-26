namespace TylerDM.MediatR.ApiGen.Test;

[Route(Route)]
public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
{
	public const string Route = "/TestingRoute";

	public Task<TestResponse> Handle(TestRequest request, CancellationToken cancellationToken) =>
		Task.FromResult(new TestResponse($"Hello {request.Name}!"));
}
