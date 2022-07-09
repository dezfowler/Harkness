namespace Harkness
{
    public class MethodCall
    {
        public MethodSignature MethodSignature { get; set; }

        public object[] Args { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (MethodCall)obj;

            return MethodSignature.DeclaringType == other.MethodSignature.DeclaringType
                && MethodSignature.MethodName == other.MethodSignature.MethodName;
        }

        public override int GetHashCode()
        {
            return MethodSignature.DeclaringType.GetHashCode();
        }
    }
}
