namespace TylerDm.MediatR.ApiGen.Server.Test;

public record TestRequest(
		string Name
) : IRequest<TestResponse>;
