using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Harkness
{
    public class Scenario : IScenario
    {
        private Session _session;

        public Scenario(Session session, IName name)
        {
            _session = session;
            Name = name;
            Stopwatch = Stopwatch.StartNew();
        }

        public IName Name { get; }

        public IScope ParentScope { get; }

        public Timeline Timeline { get; set; } = new Timeline();

        public Stopwatch Stopwatch { get; }

        public Dictionary<MethodCall, int> CallCount = new Dictionary<MethodCall, int>();

        public void Dispose()
        {
            _session.SaveTimeline(this.Name, Timeline);
        }

        public T MakeProxy<T>(T proxied)
        {
            var fallback = new ProxiedObjectCaller(proxied);
            var composite = new CallInterceptorComposite(fallback, _session, this);
            return ProxyFactory<T>.Make(composite);
        }

        public MethodCallEvent GetCapturedResult(MethodCallEvent methodCall)
        {
            // TODO: Here or somewhere else we want to recurse 
            // to check parent scopes for a match too
            return Timeline.Events
                .OfType<MethodCallEvent>()
                .FirstOrDefault(capturedCall => Match(capturedCall, methodCall));
        }

        private static bool Match(MethodCallEvent left, MethodCallEvent right)
        {
            return left.Call.Equals(right.Call) && left.Occurrence == right.Occurrence;
        }

        public void SaveCapturedResult(MethodCallEvent evt)
        {
            Timeline.Events.Add(evt);
        }

        public MethodCallEvent CreateEvent(MethodBase targetMethod, object[] args)
        {
            var signature = MethodSignature.Create(targetMethod);

            var methodCall = new MethodCall
            {
                MethodSignature = signature,
                Args = args,
            };

            if (!CallCount.TryGetValue(methodCall, out int calls))
            {
                calls = 0;
            }
            calls++;
            CallCount[methodCall] = calls;

            var methodCallEvent = new MethodCallEvent
            {
                Call = methodCall,
                Time = Stopwatch.Elapsed,
                Occurrence = calls,
            };

            return methodCallEvent;
        }
    }
}
