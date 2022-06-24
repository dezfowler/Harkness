using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harkness
{

    public class Session : ICallInterceptor
    {
        public Mode Mode { get; internal set; }

        public T MakeProxy<T>(T proxied)
        {
            var fallback = new ProxiedObjectCaller(proxied);
            var composite = new CallInterceptorComposite(fallback, this);
            return ProxyFactory<T>.Make(composite);
        }

        List<object> captured = new List<object>();
        int callIndex = 0;

        public object Invoke(MethodBase targetMethod, object[] args, Func<MethodBase, object[], object> invokeFallback)
        {
            if (Mode == Mode.Record)
            {
                var returnVal = invokeFallback(targetMethod, args);
                captured.Add(returnVal);
                return returnVal;
            }
            else if (Mode == Mode.Replay)
            {
                var returnVal = captured[callIndex];
                callIndex++;
                return returnVal;
            }
            throw new Exception("Bad");
        }
    }

}
