namespace TylerDm.MediatR.ApiGen.Extensions;

public static class StringExt
{
	public static string ToCamelCase(this string str)
	{
		var chars = str.ToCharArray();
		chars[0] = char.ToLowerInvariant(chars[0]);
		return new(chars);
	}
}
