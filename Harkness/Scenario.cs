using System.Collections.Generic;

namespace Harkness
{
    public class Scenario : IScenario
    {
        private Session _session;

        public Scenario(Session session, IScope parentScope, IName name)
        {
            _session = session;
            ParentScope = parentScope;
            Name = name;
            Calls = _session.LoadCallsForScope(this);
        }

        public IName Name { get; }

        public IScope ParentScope { get; }

        public List<CapturedResult> Calls { get; set; } = new List<CapturedResult>();

        public Dictionary<MethodCallEvent, int> CallCount = new Dictionary<MethodCallEvent, int>();

        public void Dispose()
        {
            _session.SaveCallsForScope(this, Calls);
        }

        public T MakeProxy<T>(T proxied)
        {
            var fallback = new ProxiedObjectCaller(proxied);
            var composite = new CallInterceptorComposite(fallback, _session, this);
            return ProxyFactory<T>.Make(composite);
        }

        public CapturedResult GetCapturedResult(MethodCallEvent methodCall)
        {
            if (!CallCount.TryGetValue(methodCall, out int calls))
            {
                calls = 0;
            }
            calls++;
            CallCount[methodCall] = calls;
            return Calls.Find(result => result.MethodCall.Equals(methodCall) && result.Occurrence == calls);
        }

        public void SaveCapturedResult(CapturedResult result)
        {
            if (!CallCount.TryGetValue(result.MethodCall, out int calls))
            {
                calls = 0;
            }
            calls++;
            CallCount[result.MethodCall] = calls;
            result.Occurrence = calls;
            Calls.Add(result);
        }
    }
}
