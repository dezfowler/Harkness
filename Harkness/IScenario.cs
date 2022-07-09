using System;
using System.Diagnostics;
using System.Reflection;

namespace Harkness
{
    public interface IScenario : IScoped, IDisposable
    {
        Stopwatch Stopwatch { get; }
        T MakeProxy<T>(T proxied);
        void SaveCapturedResult(MethodCallEvent result);
        MethodCallEvent GetCapturedResult(MethodCallEvent methodCall);
        MethodCallEvent CreateEvent(MethodBase targetMethod, object[] args);
    }
}
