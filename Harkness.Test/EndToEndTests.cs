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
            var proxy = session.RootScope.MakeProxy<IDateTimeProvider>(provider);
            
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
            var proxy = session.RootScope.MakeProxy<IDateTimeProvider>(provider);
            
            session.Mode = Mode.Record;
            var realValue1 = proxy.GetUtcNow();
            var realValue2 = proxy.GetUtcNow();

            session.Mode = Mode.Replay;
            var recordedValue1 = proxy.GetUtcNow();
            var recordedValue2 = proxy.GetUtcNow();

            Assert.Equal(realValue1, recordedValue1);
            Assert.Equal(realValue2, recordedValue2);
        }

        [Fact]
        public void EndToEnd_WhenMultipleScopedCalls_ThenReturnsInScope()
        {
            var session = new Session();
            var provider = new DateTimeProvider();            
            
            session.Mode = Mode.Record;
            DateTime realValue1, realValue2;

            using (var scope = session.BeginScope("Test1"))
            {
                var proxy = scope.MakeProxy<IDateTimeProvider>(provider);
                realValue1 = proxy.GetUtcNow();
            }
            
            using (var scope = session.BeginScope("Test2"))
            {
                var proxy = scope.MakeProxy<IDateTimeProvider>(provider);
                realValue2 = proxy.GetUtcNow();
            }

            session.Mode = Mode.Replay;
            DateTime recordedValue1, recordedValue2;
            using (var scope = session.BeginScope("Test1"))
            {
                var proxy = scope.MakeProxy<IDateTimeProvider>(provider);
                recordedValue1 = proxy.GetUtcNow();
            }
            
            using (var scope = session.BeginScope("Test2"))
            {
                var proxy = scope.MakeProxy<IDateTimeProvider>(provider);
                recordedValue2 = proxy.GetUtcNow();
            }

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
