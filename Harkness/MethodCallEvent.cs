using System;

namespace Harkness
{
    public class MethodCallEvent
    {   
        public MethodSignature MethodSignature { get; set; }

        public object[] Args { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (MethodCallEvent)obj;
            
            return MethodSignature.DeclaringType == other.MethodSignature.DeclaringType
                && MethodSignature.MethodName == other.MethodSignature.MethodName;
        }
        
        public override int GetHashCode()
        {
            return MethodSignature.DeclaringType.GetHashCode();
        }

        public static MethodCallEvent Create(MethodSignature methodSignature, object[] args)
        {
            if (methodSignature.ParameterTypes.Length != args.Length) 
            {
                throw new Exception("Mismatch arguments");
            }

            return new MethodCallEvent
            {
                MethodSignature = methodSignature,
                Args = args,
            };
        }
    }
}
