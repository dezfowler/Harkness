using System.Collections.Generic;

namespace Harkness
{
    public class Scope : IScope
    {
        private Session _session;

        public Scope(Session session, IScope parentScope, IName name)
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

        public IScope BeginScope(IName scopeName)
        {
            return new Scope(this._session, this, scopeName);
        }

        public IScenario BeginScenario(IName scenarioName)
        {
            return new Scenario(this._session, this, scenarioName);
        }
    }
}
