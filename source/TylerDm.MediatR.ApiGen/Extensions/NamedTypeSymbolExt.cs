namespace TylerDm.MediatR.ApiGen.Extensions;

public static class NamedTypeSymbolExt
{
	public static AttributeData? GetAttribute(this INamedTypeSymbol symbol, INamedTypeSymbol attributeSymbol) =>
			symbol.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeSymbol));
}
