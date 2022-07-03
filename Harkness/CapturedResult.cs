namespace Harkness
{
    public class CapturedResult
    {
        public MethodCallEvent MethodCall { get; set; }

        public int Occurrence { get; set; }

        public object ReturnVal { get; set; }

        public static CapturedResult Capture(MethodCallEvent methodCall, object returnVal)
        {
            return new CapturedResult
            {
                MethodCall = methodCall,
                ReturnVal = returnVal,
            };
        }
    }
}
