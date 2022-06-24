using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Harkness.Test
{
    public class EndToEndTests
    {
        [Fact]
        public void EndToEnd_WhenSameCall_ThenReturnsRecorded()
        {
            var session = new Session();
            var provider = new DateTimeProvider();
            var proxy = session.MakeProxy<IDateTimeProvider>(provider);
            
            session.Mode = Mode.Record;
            var realValue = proxy.GetUtcNow();

            session.Mode = Mode.Replay;
            var recordedValue = proxy.GetUtcNow();

            Assert.Equal(realValue, recordedValue);
        }

        [Fact]
        public void EndToEnd_WhenMultipleCalls_ThenReturnsInOrder()
        {
            var session = new Session();
            var provider = new DateTimeProvider();
            var proxy = session.MakeProxy<IDateTimeProvider>(provider);
            
            session.Mode = Mode.Record;
            var realValue1 = proxy.GetUtcNow();
            var realValue2 = proxy.GetUtcNow();

            session.Mode = Mode.Replay;
            var recordedValue1 = proxy.GetUtcNow();
            var recordedValue2 = proxy.GetUtcNow();

            Assert.Equal(realValue1, recordedValue1);
            Assert.Equal(realValue2, recordedValue2);
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
