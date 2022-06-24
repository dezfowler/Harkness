using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Harkness.Test
{
    public class CallInterceptorTests
    {
        [Fact]
        public void MethodMissing_passes_through_for_proxied_type()
        {
            var mock = Mock.Of<ISomeType>();
            Mock.Get(mock).Setup(m => m.CallMe(5)).Returns("called!");

            var interceptor = new CallInterceptor(mock);
            var proxy = ProxyFactory<ISomeType>.Make(interceptor);
            var result = proxy.CallMe(5);

            Assert.Equal("called!", result);
            Mock.Get(mock).Verify(m => m.CallMe(5), Times.Once());
        }
    }
}
