namespace Harkness
{
    public class MethodCallEvent : TimelineEvent
    {
        public MethodCall Call { get; set; }
        public CapturedResult Result { get; set; }
    }
}
