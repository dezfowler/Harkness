using System;
using System.Collections.Generic;

namespace Harkness
{
    public interface IScope : IScoped, IDisposable
    {
        IScope BeginScope(IName scopeName);
        IScenario BeginScenario(IName scenarioName);
        CapturedResult GetCapturedResult(MethodCallEvent methodCall);
    }
}
