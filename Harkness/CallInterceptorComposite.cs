using System.Reflection;

namespace Harkness
{
    public class CallInterceptorComposite : IMethodMissing
    {
        private readonly IMethodMissing _fallback;
        private readonly ICallInterceptor _interceptor;
        private readonly IScenario _scenario;

        public CallInterceptorComposite(IMethodMissing fallback, ICallInterceptor session, IScenario scenario)
        {
            _fallback = fallback;
            _interceptor = session;
            _scenario = scenario;
        }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            return _interceptor.Invoke(targetMethod, args, _fallback.Invoke, _scenario);
        }
    }
}
