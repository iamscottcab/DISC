using NUnit.Framework;
using System;

namespace DISC.Tests
{
    public class GetTests
    {
        IRootDIScope container;

        [SetUp]
        public void BeforeEach()
        {
            container = DI.CreateRootScope();
        }

        [TearDown]
        public void AfterEach()
        {
            DI.ClearMainScope();
        }

        #region Singletons

        [Test]
        public void Getting_Singleton_Class_Not_Registered_Should_Throw()
        {
            try
            {
                container.GetService<BasicClass>();
            }
            catch (NullReferenceException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Getting_Singleton_Class_Registered_Should_Not_Throw()
        {
            container.RegisterSingleton<BasicClass>();
            container.GetService<BasicClass>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Singleton_Of_Same_Class_Should_Not_Throw()
        {
            container.RegisterSingleton<BasicClass>();
            container.GetService<BasicClass>();
            container.GetService<BasicClass>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Singleton_Of_Same_Class_Resolve_Same_Object()
        {
            container.RegisterSingleton<BasicClass>();
            var singletonOne = container.GetService<BasicClass>();
            var singletonTwo =  container.GetService<BasicClass>();
            Assert.AreEqual(singletonOne, singletonTwo);
        }

        [Test]
        public void Getting_Multiple_Singleton_Of_Same_Class_Across_Scopes_Resolve_Same_Object()
        {
            container.RegisterSingleton<BasicClass>();
            var singletonOne = container.GetService<BasicClass>();

            var newScope = container.GetService<IScopeProvider>().CreateScope();
            var singletonTwo = newScope.GetService<BasicClass>();

            Assert.AreEqual(singletonOne, singletonTwo);
        }

        [Test]
        public void Getting_Singleton_Interface_Not_Registered_Should_Throw()
        {
            try
            {
                container.GetService<ISomeInterface>();
            }
            catch (NullReferenceException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Getting_Singleton_Interface_Registered_Should_Not_Throw()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();
            container.GetService<ISomeInterface>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Singleton_Of_Same_Interface_Should_Not_Throw()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();
            container.GetService<ISomeInterface>();
            container.GetService<ISomeInterface>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Singleton_Of_Same_Interface_Resolve_Same_Object()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();
            var singletonOne = container.GetService<ISomeInterface>();
            var singletonTwo = container.GetService<ISomeInterface>();
            Assert.AreEqual(singletonOne, singletonTwo);
        }

        [Test]
        public void Getting_Multiple_Singleton_Of_Same_Interface_Across_Scopes_Resolve_Same_Object()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();
            var singletonOne = container.GetService<ISomeInterface>();

            var newScope = container.GetService<IScopeProvider>().CreateScope();
            var singletonTwo = newScope.GetService<ISomeInterface>();

            Assert.AreEqual(singletonOne, singletonTwo);
        }

        #endregion

        #region Transients

        [Test]
        public void Getting_Transient_Class_Not_Registered_Should_Throw()
        {
            try
            {
                container.GetService<BasicClass>();
            }
            catch (NullReferenceException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Getting_Transient_Class_Registered_Should_Not_Throw()
        {
            container.RegisterTransient<BasicClass>();
            container.GetService<BasicClass>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Transient_Of_Same_Class_Should_Not_Throw()
        {
            container.RegisterTransient<BasicClass>();
            container.GetService<BasicClass>();
            container.GetService<BasicClass>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Transient_Of_Same_Class_Resolve_Different_Object()
        {
            container.RegisterTransient<BasicClass>();
            var TransientOne = container.GetService<BasicClass>();
            var TransientTwo = container.GetService<BasicClass>();
            Assert.AreNotEqual(TransientOne, TransientTwo);
        }

        [Test]
        public void Getting_Multiple_Transient_Of_Same_Class_Across_Scopes_Resolve_Different_Object()
        {
            container.RegisterTransient<BasicClass>();
            var TransientOne = container.GetService<BasicClass>();

            var newScope = container.GetService<IScopeProvider>().CreateScope();
            var TransientTwo = newScope.GetService<BasicClass>();

            Assert.AreNotEqual(TransientOne, TransientTwo);
        }

        [Test]
        public void Getting_Transient_Interface_Not_Registered_Should_Throw()
        {
            try
            {
                container.GetService<ISomeInterface>();
            }
            catch (NullReferenceException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Getting_Transient_Interface_Registered_Should_Not_Throw()
        {
            container.RegisterTransient<ISomeInterface, SomeClassWithInterface>();
            container.GetService<ISomeInterface>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Transient_Of_Same_Interface_Should_Not_Throw()
        {
            container.RegisterTransient<ISomeInterface, SomeClassWithInterface>();
            container.GetService<ISomeInterface>();
            container.GetService<ISomeInterface>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Transient_Of_Same_Interface_Resolve_Different_Object()
        {
            container.RegisterTransient<ISomeInterface, SomeClassWithInterface>();
            var TransientOne = container.GetService<ISomeInterface>();
            var TransientTwo = container.GetService<ISomeInterface>();
            Assert.AreNotEqual(TransientOne, TransientTwo);
        }

        [Test]
        public void Getting_Multiple_Transient_Of_Same_Interface_Across_Scopes_Resolve_Different_Object()
        {
            container.RegisterTransient<ISomeInterface, SomeClassWithInterface>();
            var TransientOne = container.GetService<ISomeInterface>();

            var newScope = container.GetService<IScopeProvider>().CreateScope();
            var TransientTwo = newScope.GetService<ISomeInterface>();

            Assert.AreNotEqual(TransientOne, TransientTwo);
        }

        #endregion
    }
}
