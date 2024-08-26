namespace TylerDM.MediatR.ApiGen.Test;

public record TestRequest(
		string Name
) : IRequest<TestResponse>;
