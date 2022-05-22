using NUnit.Framework;
using System;

namespace DISC.Tests.RegisterTests
{
    public class RegisterTests
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

        #region Bootstrapping

        [Test]
        public void Registering_Multiple_Entry_Points_Should_Throw()
        {
            container.RegisterEntryPoint<BasicClass>();
            try
            {
                container.RegisterEntryPoint<BasicClass>();
            }
            catch (NotSupportedException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Resolving_Entry_Point_Not_Registered_Should_Throw()
        {
            try
            {
                container.ResolveEntryPoint<BasicClass>();
            }
            catch (NullReferenceException)
            {
                Assert.That(true);
            }
        }


        [Test]
        public void Resolving_Different_Entry_Point_To_Registered_Should_Throw()
        {
            container.RegisterEntryPoint<BasicClass>();
            try
            {
                container.ResolveEntryPoint<SomeOtherBasicClass>();
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Overriding_Registered_Lifetime_Should_Not_Throw()
        {
            container.RegisterTransient<BasicClass>();
            container.RegisterSingleton<BasicClass>();
            Assert.That(true);
        }

        #endregion

        #region Singletons

        [Test]
        public void Registering_Abstract_Singleton_Class_Should_Throw()
        {
            try
            {
                container.RegisterSingleton<BasicAbstractClass>();
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Registering_Singleton_Class_Should_Not_Throw()
        {
            container.RegisterSingleton<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Same_Singleton_Class_Multiple_Times_Should_Not_Throw()
        {
            container.RegisterSingleton<BasicClass>();
            container.RegisterSingleton<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Singleton_Interface_Should_Not_Throw()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Same_Singleton_Interface_Multiple_Times_Should_Not_Throw()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();
            container.RegisterSingleton<ISomeInterface, SomeOtherClassWithInterface>();

            Assert.That(true);
        }

        #endregion

        #region Transients

        [Test]
        public void Registering_Abstract_Transient_Class_Should_Throw()
        {
            try
            {
                container.RegisterTransient<BasicAbstractClass>();
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
        }

        [Test]
        public void Registering_Transient_Class_Should_Not_Throw()
        {
            container.RegisterTransient<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Same_Transient_Class_Multiple_Times_Should_Not_Throw()
        {
            container.RegisterTransient<BasicClass>();
            container.RegisterTransient<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Transient_Interface_Should_Not_Throw()
        {
            container.RegisterTransient<ISomeInterface, SomeClassWithInterface>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Same_Transient_Interface_Multiple_Times_Should_Not_Throw()
        {
            container.RegisterTransient<ISomeInterface, SomeClassWithInterface>();
            container.RegisterTransient<ISomeInterface, SomeOtherClassWithInterface>();

            Assert.That(true);
        }

        #endregion
    }
}
