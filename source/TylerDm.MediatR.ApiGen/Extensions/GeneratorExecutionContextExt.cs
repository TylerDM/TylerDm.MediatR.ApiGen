namespace TylerDm.MediatR.ApiGen.Extensions;

public static class GeneratorExecutionContextExt
{
	public static void AddClassFile(this GeneratorExecutionContext context, string className, string utf8Code) =>
			context.AddSource($"{className}.g.cs", SourceText.From(utf8Code, Encoding.UTF8));
}
