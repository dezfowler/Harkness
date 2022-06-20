using System.Reflection;

namespace Harkness
{
    public interface IMethodMissing
    {
        object Invoke(MethodBase targetMethod, object[] args);
    }
}
