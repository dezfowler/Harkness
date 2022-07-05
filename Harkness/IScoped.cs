namespace Harkness
{
    public interface IScoped
    {
        IName Name { get; }
        IScope ParentScope { get; }
    }
}
