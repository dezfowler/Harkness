using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Harkness.Test
{
    public class EndToEndTests
    {
        [Fact]
        public void EndToEnd()
        {
            var session = new Session();
            session.Mode = Mode.Record;
            var provider = new DateTimeProvider();
            var proxy = session.MakeProxy<IDateTimeProvider>(provider);
            var realValue = proxy.GetUtcNow();

            session.Mode = Mode.Replay;
            var recordedValue = proxy.GetUtcNow();

            Assert.Equal(realValue, recordedValue);
        }
    }

    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
