using System;
using System.Collections.Generic;

namespace Harkness
{
    public interface IScope : IDisposable
    {
        T MakeProxy<T>(T proxied);
        void SaveCapturedResult(CapturedResult result);
        CapturedResult GetCapturedResult(MethodCallEvent methodCall);
    }
}
