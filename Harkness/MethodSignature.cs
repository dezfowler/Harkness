using System;
using System.Linq;
using System.Reflection;

namespace Harkness
{
    public class MethodSignature
    {
        public Type DeclaringType { get; set; }
        public string MethodName { get; set; }
        public Type[] ParameterTypes { get; set; }
        public Type ReturnType { get; set; }

        public static MethodSignature Create(MethodBase targetMethod)
        {
            var methodInfo = (MethodInfo)targetMethod;
            
            var parameterInfos = methodInfo.GetParameters();

            if (parameterInfos.Any(p => p.IsOut))
            {
                throw new NotSupportedException("Out parameters not yet supported");
            }

            if (parameterInfos.Any(p => typeof(Delegate).IsAssignableFrom(p.ParameterType)))
            {
                throw new NotSupportedException("Delegate parameters not yet supported");
            }
            
            return new MethodSignature
            {
                DeclaringType = methodInfo.DeclaringType,
                MethodName = methodInfo.Name,
                ParameterTypes = parameterInfos
                    .Select(p => p.ParameterType)
                    .ToArray(),
                ReturnType = methodInfo.ReturnType,
            };
        }
    }
}
