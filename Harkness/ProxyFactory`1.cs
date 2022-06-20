using System;
using System.Reflection;

namespace Harkness
{
    public class ProxyFactory<T>
    {
#if NET40
        public static T Make(IMethodMissing methodMissing)
        {
            var proxy = new Proxy();
            proxy.InvokeFunc = methodMissing.Invoke;
            return (T)proxy.GetTransparentProxy();
        }

        class Proxy : System.Runtime.Remoting.Proxies.RealProxy
        {
            internal Func<MethodBase, object[], object> InvokeFunc { get; set; }

            public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage msg)
            {
                var methodCallMessage = (System.Runtime.Remoting.Messaging.IMethodCallMessage)msg;
                var retVal = InvokeFunc(methodCallMessage.MethodBase, methodCallMessage.Args);
                var returnMessage = new System.Runtime.Remoting.Messaging.ReturnMessage(retVal, null, 0, methodCallMessage.LogicalCallContext, methodCallMessage);
                return returnMessage;
            }
        }
#else
        public static T Make(IMethodMissing methodMissing)
        {
            var proxy = DispatchProxy.Create<T, Proxy>();
            (proxy as Proxy).InvokeFunc = methodMissing.Invoke;
            return (T)proxy;
        }
        
        public class Proxy : DispatchProxy
        {
            internal Func<MethodBase, object[], object> InvokeFunc { get; set; }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                return InvokeFunc(targetMethod, args);
            }
        }
#endif
    }
}
