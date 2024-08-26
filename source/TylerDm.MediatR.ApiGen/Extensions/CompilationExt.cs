using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TylerDm.MediatR.ApiGen.Extensions;

public static class CompilationExt
{
	public static INamedTypeSymbol GetSymbol(this Compilation compilation, Func<INamedTypeSymbol, bool> predicate) =>
			compilation.GlobalNamespace.GetNamespaceMembers().SelectMany(x => x.GetTypeMembers()).First(predicate);

	public static IEnumerable<INamedTypeSymbol> GetSymbols(this Compilation compilation) =>
			compilation.GlobalNamespace.GetNamespaceMembers().SelectMany(x => x.GetTypeMembers());

	public static IEnumerable<INamedTypeSymbol> GetClassTypeSymbols(this Compilation compilation)
	{
		foreach (var syntaxTree in compilation.SyntaxTrees)
		{
			var semanticModel = compilation.GetSemanticModel(syntaxTree);
			var root = syntaxTree.GetRoot();
			foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
				if (semanticModel.GetDeclaredSymbol(classDeclaration) is INamedTypeSymbol symbol)
					yield return symbol;
		}
	}

	public static INamedTypeSymbol GetTypeByType<T>(this Compilation compilation) =>
			compilation.GetSymbolByType(typeof(T));

	public static INamedTypeSymbol GetSymbolByType(this Compilation compilation, Type type) =>
			compilation.GetTypeByMetadataName($"{type.Namespace}.{type.Name}") ??
			throw new Exception("Could not find type in compilation.");
}
