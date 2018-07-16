using System;
using System.Reflection;
using Xunit;

namespace Harkness.Test
{
    public class ProxyFactoryTests
    {
        [Fact]
        public void Should_work_with_method_that_returns_a_value()
        {
            var mm = new MethodMissingStub();
            mm.Return = 5;
            
            var proxy = ProxyFactory<ITestService>.Make(mm);
            
            var result = proxy.SomeMethod("hello world", 7);

            Assert.Equal("SomeMethod", mm.MethodCalled);
            Assert.Equal(new object[] { "hello world", 7 }, mm.Arguments);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Should_work_with_method_with_no_return()
        {
            var mm = new MethodMissingStub();
            mm.Return = typeof(void);

            var proxy = ProxyFactory<ITestService>.Make(mm);

            proxy.NoReturn("hello world");

            Assert.Equal("NoReturn", mm.MethodCalled);
            Assert.Equal(new object[] { "hello world" }, mm.Arguments);
        }

        [Fact]
        public void Should_work_with_property_setter()
        {
            var mm = new MethodMissingStub();
            mm.Return = typeof(void);

            var proxy = ProxyFactory<ITestService>.Make(mm);

            proxy.MyProperty = 7;

            Assert.Equal("set_MyProperty", mm.MethodCalled);
            Assert.Equal(new object[] { 7 }, mm.Arguments);
        }

        [Fact]
        public void Should_work_with_property_getter()
        {
            var mm = new MethodMissingStub();
            mm.Return = 7;

            var proxy = ProxyFactory<ITestService>.Make(mm);

            var result = proxy.MyProperty;

            Assert.Equal("get_MyProperty", mm.MethodCalled);
            Assert.Equal(new object[] { }, mm.Arguments);
            Assert.Equal(7, result);
        }

        [Fact]
        public void Should_work_with_indexed_property_setter()
        {
            var mm = new MethodMissingStub();
            mm.Return = typeof(void);

            var proxy = ProxyFactory<ITestService>.Make(mm);

            proxy["hello world"] = 7;

            Assert.Equal("set_Item", mm.MethodCalled);
            Assert.Equal(new object[] { "hello world", 7 }, mm.Arguments);
        }

        [Fact]
        public void Should_work_with_indexed_property_getter()
        {
            var mm = new MethodMissingStub();
            mm.Return = 7;

            var proxy = ProxyFactory<ITestService>.Make(mm);

            var result = proxy["hello world"];

            Assert.Equal("get_Item", mm.MethodCalled);
            Assert.Equal(new object[] { "hello world" }, mm.Arguments);
            Assert.Equal(7, result);
        }
    }

    public interface ITestService
    {
        int SomeMethod(string words, int num);

        void NoReturn(string blah);

        int MyProperty { get; set; }

        int this[string val] { get; set; }
    }

    class MethodMissingStub : IMethodMissing
    {
        public string MethodCalled { get; set; }
        public object[] Arguments { get; set; }
        public object Return { get; set; }

        public object Invoke(MethodBase targetMethod, object[] args)
        {
            MethodCalled = targetMethod.Name;
            Arguments = args;
            return Return;
        }
    }
}
