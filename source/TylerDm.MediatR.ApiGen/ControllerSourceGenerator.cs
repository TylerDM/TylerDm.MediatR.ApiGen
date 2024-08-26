using System.Diagnostics;

namespace TylerDm.MediatR.ApiGen;

[Generator]
public class ControllerSourceGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context)
	{
#if DEBUG
		Debugger.Launch();
#endif
	}

	public void Execute(GeneratorExecutionContext context)
	{
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

	private static IEnumerable<MediatorHandlerControllerDefinition> getDefinitions(Compilation compilation) =>
		from handler in compilation.GetClassTypeSymbols()
		let route = getRoute(handler)
		where route is not null
		let interfaceSymbol = getInterfaceSymbol(handler)
		where interfaceSymbol is not null
		let typeArguments = interfaceSymbol.TypeArguments
		let requestSymbol = typeArguments[0]
		let responseSymbol = typeArguments[1]
		select new MediatorHandlerControllerDefinition(handler, route, requestSymbol, responseSymbol);

	private static string? getRoute(INamedTypeSymbol handlerTypeSymbol)
	{
		var attribute = handlerTypeSymbol.GetAttribute(x =>
		{
			var attribute = x.AttributeClass;
			if (attribute is null) return false;

			return
				attribute.Name == "RouteAttribute" &&
				attribute.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::Microsoft.AspNetCore.Mvc";
		});
		if (attribute is null) return null;

		if (attribute.ConstructorArguments.Length == 0) return "[controller]";
		return (string)attribute.ConstructorArguments[0].Value!;
	}

	private static INamedTypeSymbol? getInterfaceSymbol(INamedTypeSymbol handlerTypeSymbol) =>
		handlerTypeSymbol.AllInterfaces.FirstOrDefault(x =>
			x.IsGenericType &&
			x.ConstructedFrom.Name.StartsWith("IRequestHandler") &&
			x.ConstructedFrom.ContainingNamespace.Name == "MediatR"
		);
}