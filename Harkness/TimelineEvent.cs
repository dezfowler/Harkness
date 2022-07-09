using System;

namespace Harkness
{
    // could be a method call, callback, event, observable stream item
    public class TimelineEvent
    {
        public TimeSpan Time { get; set; }
        public int Occurrence { get; set; }
    }
}
