namespace TylerDm.MediatR.ApiGen;

public class MediatorHandlerControllerDefinition
{
	public INamedTypeSymbol HandlerTypeSymbol { get; }
	public string Route { get; }
	public ITypeSymbol RequestTypeSymbol { get; }
	public ITypeSymbol ResponseTypeSymbol { get; }
	public string ClassName { get; }

	public MediatorHandlerControllerDefinition(INamedTypeSymbol handlerTypeSymbol, string route, ITypeSymbol requestTypeSymbol, ITypeSymbol responseTypeSymbol)
	{
		HandlerTypeSymbol = handlerTypeSymbol;
		Route = route;
		RequestTypeSymbol = requestTypeSymbol;
		ResponseTypeSymbol = responseTypeSymbol;

		ClassName = $"{HandlerTypeSymbol.Name}Controller";
	}
}
