namespace TylerDm.MediatR.ApiGen.Extensions;

public static class CollectionExt
{
	public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
	{
		foreach (var value in values)
			collection.Add(value);
	}
}
