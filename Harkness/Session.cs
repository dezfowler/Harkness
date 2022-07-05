using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Harkness
{
    public class Session : ICallInterceptor
    {
        public Session()
        {
            Reset();
        }

        public Mode Mode { get; set; }

        public Speed ReplaySpeed { get; set; }

        private IScope RootScope { get; set; }

        private Dictionary<string, List<CapturedResult>> Store { get; set; } = new Dictionary<string, List<CapturedResult>>();

        public object Invoke(
            MethodBase targetMethod, 
            object[] args, 
            Func<MethodBase, object[], object> invokeFallback, 
            IScenario scenario)
        {
            var signature = MethodSignature.Create(targetMethod);
            var methodCall = MethodCallEvent.Create(signature, args);
            if (Mode == Mode.Record)
            {
                var returnVal = invokeFallback(targetMethod, args);
                var result = CapturedResult.Capture(methodCall, returnVal);
                scenario.SaveCapturedResult(result);
                return returnVal;
            }
            else if (Mode == Mode.Replay)
            {
                var capturedResult = scenario.GetCapturedResult(methodCall);
                if (capturedResult == null) throw new Exception("No matching call found");
                return capturedResult.ReturnVal;
            }

            throw new Exception("Bad");
        }

        public List<CapturedResult> LoadCallsForScope(IScoped scope)
        {
            if(Mode == Mode.Record) return new List<CapturedResult>();
            Store.TryGetValue(scope.Name.GetName(), out List<CapturedResult> saved);
            return saved ?? new List<CapturedResult>();
        }

        public void SaveCallsForScope(IScoped scope, List<CapturedResult> calls)
        {
            if(Mode != Mode.Record) return;

            Store[scope.Name.GetName()] = calls;
        }

        public IScope BeginScope(IName scopeName)
        {
            return new Scope(this, this.RootScope, scopeName);
        }

        public IScenario BeginScenario(IName scenarioName)
        {
            return new Scenario(this, this.RootScope, scenarioName);
        }

        private void Reset()
        {
            RootScope = new Scope(this, null, new StringName("Root"));
        }
    }

    public enum Speed
    {
        Instant = 0,
        RealTime = 1,
        TwoTimes = 2,
        FiveTimes = 5,
        TenTimes = 10,
    }
}
