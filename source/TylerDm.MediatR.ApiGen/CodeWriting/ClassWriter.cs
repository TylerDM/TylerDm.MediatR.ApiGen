namespace TylerDm.MediatR.ApiGen.CodeWriting;

public class ClassWriter
{
	private readonly StringBuilder _sb = new();

	private int indent = 0;

	public void WriteMethodDeclaration(string returnType, string name, string[] parameters, bool expressionBodied = false, AccessModifiers accessModifier = AccessModifiers.Public)
	{
		writeIndent();
		write($"{accessModifier.ToString().ToLowerInvariant()} {returnType} {name}(");
		foreach (var parameter in parameters)
			write($"{parameter}, ");
		truncateEnd(2);
		write(')');
		if (expressionBodied)
			write(" =>");
		endLine();
	}

	public void WriteBlock(Action<ClassWriter> action)
	{
		writeLine("{");
		WriteIndented(action);
		writeLine("}");
	}

	public void WriteIndentedLine(string line) =>
			WriteIndented(writer => writer.writeLine(line));

	public void WriteIndented(Action<ClassWriter> action)
	{
		indent++;
		action(this);
		indent--;
	}

	public void WriteAttribute(string attributeName, params string[] parameters)
	{
		writeIndent();
		write('[');
		write(attributeName);
		if (parameters.Length > 0)
		{
			write('(');
			foreach (string parameter in parameters)
				write($"{parameter}, ");
			truncateEnd(2);
			write(')');
		}
		write(']');
		endLine();
	}

	public void WriteClassDeclaration(string className, string[] parameters, AccessModifiers accessModifier = AccessModifiers.Public, string? baseClass = null)
	{
		write($"{accessModifier.ToString().ToLowerInvariant()} class {className}");
		if (parameters.Length > 0)
		{
			write('(');
			foreach (string parameter in parameters)
				write($"{parameter}, ");
			truncateEnd(2);
			write(')');
		}
		if (baseClass is not null)
			write($" : {baseClass}");
		endLine();
	}

	public void WriteUsing(string ns) =>
			writeLine($"using {ns};");

	public void WriteNamespace(string ns) =>
			writeLine($"namespace {ns};");

	public void WriteUsings(params string[] nss) =>
			WriteUsings(new NamespaceCollection(nss));

	public void WriteUsings(NamespaceCollection nss)
	{
		foreach (var ns in nss.OrderBy(x => x))
			WriteUsing(ns);
	}

	public void WriteEmptyLine() =>
			_sb.AppendLine();

	public override string ToString() =>
			_sb.ToString();

	private void truncateEnd(int count) =>
			_sb.Remove(_sb.Length - count, count);

	private void write(object value) =>
			write(value.ToString() ?? "");

	private void write(string value) =>
			_sb.Append(value);

	private void write(char ch) =>
			_sb.Append(ch);

	private void endLine() =>
			write('\n');

	private void writeLine(string text)
	{
		writeIndent();
		write(text);
		endLine();
	}

	private void writeLine(char ch)
	{
		writeIndent();
		write(ch);
		endLine();
	}

	private void writeIndent()
	{
		for (int i = 0; i < indent; i++)
			write('\t');
	}
}
