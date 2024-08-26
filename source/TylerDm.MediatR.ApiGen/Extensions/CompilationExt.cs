namespace TylerDm.MediatR.ApiGen.Extensions;

public static class CompilationExt
{
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
}
