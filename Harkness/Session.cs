using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harkness
{
    public class Session : ICallInterceptor
    {
        public Session()
        {
            RootScope = new Scope(this, null);
        }

        public Mode Mode { get; internal set; }

        public IScope RootScope { get; set; }

        List<object> captured = new List<object>();
        int callIndex = 0;

        public object Invoke(
            MethodBase targetMethod, 
            object[] args, 
            Func<MethodBase, object[], object> invokeFallback, 
            IScope scope)
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

        public IScope BeginScope(string scopeName)
        {
            return new Scope(this, this.RootScope);
        }
    }

    public interface IScope : IDisposable
    {
        T MakeProxy<T>(T proxied);
    }

    public class Scope : IScope
    {
        private Session _session;
        private IScope _parentScope;

        public Scope(Session session, IScope parentScope)
        {
            _session = session;
            _parentScope = parentScope;
        }

        public void Dispose()
        {
        }

        public T MakeProxy<T>(T proxied)
        {
            var fallback = new ProxiedObjectCaller(proxied);
            var composite = new CallInterceptorComposite(fallback, _session, this);
            return ProxyFactory<T>.Make(composite);
        }
    }
}
