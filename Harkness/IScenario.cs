using System;

namespace Harkness
{
    public interface IScenario : IScoped, IDisposable
    {
        T MakeProxy<T>(T proxied);
        void SaveCapturedResult(CapturedResult result);
        CapturedResult GetCapturedResult(MethodCallEvent methodCall);
    }
}
