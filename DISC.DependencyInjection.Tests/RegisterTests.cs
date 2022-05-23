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
            DI.ClearRootScope();
        }

        #region Bootstrapping

        [Test]
        public void Registering_Multiple_Entry_Points_Should_Throw()
        {
            container.RegisterEntryPoint<BasicClass>();
            try
            {
                container.RegisterEntryPoint<BasicClass>();
                Assert.That(false);
            }
            catch (NotSupportedException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Resolving_Entry_Point_Not_Registered_Should_Throw()
        {
            try
            {
                container.ResolveEntryPoint<BasicClass>();
                Assert.That(false);
            }
            catch (NullReferenceException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }


        [Test]
        public void Resolving_Different_Entry_Point_To_Registered_Should_Throw()
        {
            container.RegisterEntryPoint<BasicClass>();
            try
            {
                container.ResolveEntryPoint<SomeOtherBasicClass>();
                Assert.That(false);
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
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
                Assert.That(false);
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
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
                Assert.That(false);
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
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

        #region Scoped

        [Test]
        public void Registering_Abstract_Scoped_Class_Should_Throw()
        {
            try
            {
                container.RegisterScoped<BasicAbstractClass>();
                Assert.That(false);
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Registering_Scoped_Class_Should_Not_Throw()
        {
            container.RegisterScoped<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Same_Scoped_Class_Multiple_Times_Should_Not_Throw()
        {
            container.RegisterScoped<BasicClass>();
            container.RegisterScoped<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Scoped_Interface_Should_Not_Throw()
        {
            container.RegisterScoped<ISomeInterface, SomeClassWithInterface>();

            Assert.That(true);
        }

        [Test]
        public void Registering_Same_Scoped_Interface_Multiple_Times_Should_Not_Throw()
        {
            container.RegisterScoped<ISomeInterface, SomeClassWithInterface>();
            container.RegisterScoped<ISomeInterface, SomeOtherClassWithInterface>();

            Assert.That(true);
        }

        #endregion

        #region Registering via Types

        [Test]
        public void Registering_Open_Generics_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(IGenericInterface<>), typeof(GenericClassWithInterface<>));
        }

        [Test]
        public void Registering_Class_By_Type_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(BasicClass));
        }

        [Test]
        public void Registering_Class_By_Type_With_Factory_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(BasicClass), () => new BasicClass());
        }

        [Test]
        public void Registering_Interface_By_Type_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(ISomeInterface), typeof(SomeClassWithInterface));
        }

        [Test]
        public void Registering_Interface_By_Type_With_Factory_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(ISomeInterface), typeof(SomeClassWithInterface), () => new SomeClassWithInterface());
        }

        [Test]
        public void Registering_Class_With_No_Type_Should_Throw()
        {
            try
            {
                container.RegisterSingleton(null);
                Assert.That(false);
            }
            catch (ArgumentNullException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Registering_Interface_With_No_Type_Should_Throw()
        {
            try
            {
                container.RegisterSingleton(null, typeof(BasicClass));
                Assert.That(false);
            }
            catch (ArgumentNullException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Registering_With_No_Types_Should_Throw()
        {
            try
            {
                container.RegisterSingleton(null, null, null);
                Assert.That(false);
            }
            catch (ArgumentNullException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Registering_Static_Class_By_Type_Should_Throw()
        {
            try
            {
                container.RegisterSingleton(typeof(BasicStaticClass));
                Assert.That(false);
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Registering_Abstract_Class_By_Type_Should_Throw()
        {
            try
            {
                container.RegisterSingleton(typeof(BasicAbstractClass));
                Assert.That(false);
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Registering_Interface_By_Type_Should_Throw()
        {
            try
            {
                container.RegisterSingleton(typeof(ISomeInterface));
                Assert.That(false);
            }
            catch (ArgumentException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        #endregion
    }
}
