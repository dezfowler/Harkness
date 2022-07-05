using System;
using System.Reflection;

namespace Harkness
{
    public interface ICallInterceptor
    {
        object Invoke(
            MethodBase targetMethod, 
            object[] args, 
            Func<MethodBase, object[], object> invokeFallback,
            IScenario scenario);
    }

}
