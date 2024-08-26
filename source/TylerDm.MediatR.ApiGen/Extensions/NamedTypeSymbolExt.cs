namespace TylerDm.MediatR.ApiGen.Extensions;

public static class NamedTypeSymbolExt
{
	public static AttributeData? GetAttribute(this INamedTypeSymbol symbol, Func<AttributeData, bool> predicate) =>
			symbol.GetAttributes().FirstOrDefault(predicate);
}
