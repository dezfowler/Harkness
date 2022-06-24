using System;
using System.Reflection;

namespace Harkness
{
    public class CallInterceptor : IMethodMissing
    {
        private readonly object _subject;

        public CallInterceptor(object subject)
        {
            _subject = subject;
        }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            return targetMethod.Invoke(_subject, args);
        }
    }

    public class CallInterceptor2 : IMethodMissing
    {
        private readonly IMethodMissing _fallback;
        private readonly Session _session;

        public CallInterceptor2(IMethodMissing fallback, Session session)
        {
            _fallback = fallback;
            _session = session;
        }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            return _session.Invoke(targetMethod, args, _fallback.Invoke);
        }
    }

    public class Session
    {
        public Mode Mode { get; internal set; }

        public T MakeProxy<T>(T proxied)
        {
            var fallback = new CallInterceptor(proxied);
            var composite = new CallInterceptor2(fallback, this);
            return ProxyFactory<T>.Make(composite);
        }

        object captured;

        internal object Invoke(MethodBase targetMethod, object[] args, Func<MethodBase, object[], object> invokeFallback)
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


    public enum Mode
    {
        Record,
        Replay
    }

}
