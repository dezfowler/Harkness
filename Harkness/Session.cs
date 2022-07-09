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
        public Mode Mode { get; set; }

        public Speed ReplaySpeed { get; set; }

        private Dictionary<string, Timeline> Store { get; set; } = new Dictionary<string, Timeline>();

        public object Invoke(
            MethodBase targetMethod, 
            object[] args, 
            Func<MethodBase, object[], object> invokeFallback, 
            IScenario scenario)
        {
            MethodCallEvent methodCall = scenario.CreateEvent(targetMethod, args);
            
            if (Mode == Mode.Record)
            {

                // TODO: Capture all the different types of result here
                // exception, Task, Stream, Observable, etc
                // in a way that can be serialized and replayed later

                var returnVal = invokeFallback(targetMethod, args);

                var result = new CapturedResult 
                { 
                    ReturnVal = returnVal, 
                    Time = scenario.Stopwatch.Elapsed 
                };

                methodCall.Result = result;

                scenario.SaveCapturedResult(methodCall);
                return returnVal;
            }
            else if (Mode == Mode.Replay)
            {
                var capturedEvent = scenario.GetCapturedResult(methodCall);
                
                if (capturedEvent == null) 
                {
                    throw new Exception("No matching call found");
                }

                // TODO: Add delay dependent on replay speed

                // TODO: Yield different result types:
                // - bare value
                // - wrap in Task
                // - create Stream
                // - throw exception
                // - etc

                return capturedEvent.Result.ReturnVal;
            }

            throw new Exception("Bad");
        }

        public Timeline LoadTimeline(IName name)
        {
            if(Mode == Mode.Record) return new Timeline();
            Store.TryGetValue(name.GetName(), out Timeline saved);
            return saved ?? new Timeline();
        }

        public void SaveTimeline(IName name, Timeline timeline)
        {
            if(Mode != Mode.Record) return;

            Store[name.GetName()] = timeline;
        }

        public IScenario BeginScenario(IName scenarioName)
        {
            return new Scenario(this, scenarioName)
            {
                Timeline = LoadTimeline(scenarioName)
            };
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
