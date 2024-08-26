namespace TylerDm.MediatR.ApiGen;

[Generator]
public class ControllerSourceGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context) { }

	public void Execute(GeneratorExecutionContext context)
	{
		Debugger.Launch();

		foreach (var def in getDefinitions(context.Compilation))
		{
			var code = writeCode(def);
			context.AddClassFile(def.ClassName, code);
		}
	}

	private static string writeCode(MediatorHandlerControllerDefinition def)
	{
		var writer = new ClassWriter();
		writer.WriteUsings(
			"Microsoft.AspNetCore.Mvc",
			"MediatR",
			def.RequestTypeSymbol.ContainingNamespace.ToDisplayString()!,
			def.ResponseTypeSymbol.ContainingNamespace.ToDisplayString()!
		);
		writer.WriteEmptyLine();
		writer.WriteNamespace(def.HandlerTypeSymbol.ContainingNamespace.ToDisplayString());
		writer.WriteEmptyLine();
		writer.WriteAttribute("ApiController");
		writer.WriteAttribute("Route", $"\"{def.Route}\"");
		writer.WriteClassDeclaration(def.ClassName, ["IMediator _mediator"]);
		writer.WriteBlock(writer =>
		{
			writer.WriteAttribute("HttpPost");
			writer.WriteMethodDeclaration($"Task<{def.ResponseTypeSymbol.Name}>", "PostAsync", [$"{def.RequestTypeSymbol.Name} request"], expressionBodied: true);
			writer.WriteIndentedLine("_mediator.Send(request);");
		});
		return writer.ToString();
	}

	private static IEnumerable<MediatorHandlerControllerDefinition> getDefinitions(Compilation compilation)
	{
		var interfaceTypeSymbol = compilation.GetSymbol(x =>
			x.Name.StartsWith("IRequestHandler") &&
			x.ContainingNamespace.Name == "MediatR"
		);
		var routeAttributeSymbol = compilation.GetTypeByType<WebAccessibleAttribute>();

		foreach (var handlerTypeSymbol in compilation.GetClassTypeSymbols())
		{
			var attribute = handlerTypeSymbol.GetAttribute(routeAttributeSymbol);
			if (attribute is null) continue;
			var route = (string)attribute.ConstructorArguments.Single().Value!;

			var interfaceSymbol = getInterfaceSymbol(handlerTypeSymbol, interfaceTypeSymbol);
			if (interfaceSymbol is null) continue;

			var (requestSymbol, responseSymbol) = getHandlerTypes(handlerTypeSymbol, interfaceTypeSymbol);
			yield return new(handlerTypeSymbol, route, requestSymbol, responseSymbol);
		}
	}

	private static (ITypeSymbol requestType, ITypeSymbol responseType) getHandlerTypes(INamedTypeSymbol symbol, INamedTypeSymbol interfaceTypeSymbol)
	{
		foreach (var interfaceSymbol in symbol.AllInterfaces)
		{
			if (interfaceSymbol.IsGenericType == false) continue;
			if (SymbolEqualityComparer.Default.Equals(interfaceSymbol.ConstructedFrom, interfaceTypeSymbol) == false) continue;

			var typeArguments = interfaceSymbol.TypeArguments;
			return (typeArguments[0], typeArguments[1]);
		}
		throw new Exception("Failed to determine request and response types.");
	}

	private static INamedTypeSymbol? getInterfaceSymbol(INamedTypeSymbol left, ISymbol right) =>
		left.AllInterfaces.FirstOrDefault(x => x.IsGenericType && SymbolEqualityComparer.Default.Equals(x.ConstructedFrom, right));
}