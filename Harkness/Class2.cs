using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harkness
{
    public abstract class CallContext
    {
        public CallContext Parent { get; set; }

        public T As<T>() where T : CallContext
        {
            return (this as T) ?? this.Parent?.As<T>();
        }

        public CallContext Push(CallContext top)
        {
            top.Parent = this;
            return top;
        }
    }

    public class EmptyContext : CallContext
    {
    }
}
