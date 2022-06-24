using System.Reflection;

namespace Harkness
{
    public class CallInterceptorComposite : IMethodMissing
    {
        private readonly IMethodMissing _fallback;
        private readonly ICallInterceptor _interceptor;

        public CallInterceptorComposite(IMethodMissing fallback, ICallInterceptor session)
        {
            _fallback = fallback;
            _interceptor = session;
        }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            return _interceptor.Invoke(targetMethod, args, _fallback.Invoke);
        }
    }
}
