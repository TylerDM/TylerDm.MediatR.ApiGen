namespace TylerDm.MediatR.ApiGen;

[AttributeUsage(AttributeTargets.Class)]
public class WebAccessibleAttribute(string route = "") : Attribute
{
	public string Route { get; } = route;
}
