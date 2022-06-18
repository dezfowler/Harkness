using System.Reflection;

namespace Harkness
{
    public class CallInterceptor : IMethodMissing
    {
        private readonly object subject;

        public CallInterceptor(object subject)
        {
            this.subject = subject;
        }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            return targetMethod.Invoke(subject, args);
        }
    }
}
