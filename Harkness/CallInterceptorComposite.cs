using System.Reflection;

namespace Harkness
{
    public class CallInterceptorComposite : IMethodMissing
    {
        private readonly IMethodMissing _fallback;
        private readonly ICallInterceptor _interceptor;
        private readonly IScope _scope;

        public CallInterceptorComposite(IMethodMissing fallback, ICallInterceptor session, IScope scope)
        {
            _fallback = fallback;
            _interceptor = session;
            _scope = scope;
        }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            return _interceptor.Invoke(targetMethod, args, _fallback.Invoke, _scope);
        }
    }
}
