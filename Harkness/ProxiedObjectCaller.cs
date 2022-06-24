using System.Reflection;

namespace Harkness
{
    public class ProxiedObjectCaller : IMethodMissing
    {
        private readonly object _subject;

        public ProxiedObjectCaller(object subject)
        {
            _subject = subject;
        }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            return targetMethod.Invoke(_subject, args);
        }
    }
}
