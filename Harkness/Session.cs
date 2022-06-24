using System;
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

        object captured;

        public object Invoke(MethodBase targetMethod, object[] args, Func<MethodBase, object[], object> invokeFallback)
        {
            if (Mode == Mode.Record)
            {
                captured = invokeFallback(targetMethod, args);
                return captured;
            }
            else if (Mode == Mode.Replay)
            {
                return captured;
            }
            throw new Exception("Badd");
        }
    }

}
