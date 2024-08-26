namespace TylerDm.MediatR.ApiGen.CodeWriting;

public class NamespaceCollection : HashSet<string>
{
	public NamespaceCollection(IEnumerable<string> nss) : base(nss) { }
	public NamespaceCollection() { }
}