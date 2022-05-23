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

        [Test]
        public void Getting_Class_With_No_Constructors_Should_Not_Throw()
        {
            container.RegisterSingleton<BasicClass>();
            container.GetService<BasicClass>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Class_With_One_Constructor_Should_Not_Throw()
        {
            container.RegisterSingleton<ClassWithConstructor>();
            container.GetService<ClassWithConstructor>();
            Assert.That(true);
        }

        [Test]
        public void Getting_Class_With_Multiple_Constructors_Should_Throw()
        {
            container.RegisterSingleton<ClassWithMultipleConstructors>();
            try
            {
                container.GetService<ClassWithMultipleConstructors>();
                Assert.That(false);
            }
            catch (InvalidOperationException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        #region Singletons

        [Test]
        public void Getting_Singleton_Class_Not_Registered_Should_Throw()
        {
            try
            {
                container.GetService<BasicClass>();
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

        [Test]
        public void Getting_Singleton_By_Class_With_Factory_Should_Return_Correct_Object()
        {
            var obj = new ClassWithStringProperty();
            obj.Value = "Some test value";

            container.RegisterSingleton<ClassWithStringProperty>(() => obj);

            var singleton = container.GetService<ClassWithStringProperty>();
            
            Assert.NotNull(singleton.Value);
            Assert.AreEqual(obj, singleton);
        }

        [Test]
        public void Getting_Singleton_By_Interface_With_Factory_Should_Return_Correct_Object()
        {
            var obj = new ClassWithStringProperty();
            obj.Value = "Some test value";

            container.RegisterSingleton<IClassWithStringProperty, ClassWithStringProperty>(() => obj);

            var singleton = container.GetService<IClassWithStringProperty>();

            Assert.NotNull(singleton.Value);
            Assert.AreEqual(obj, singleton);
        }

        #endregion

        #region Transients

        [Test]
        public void Getting_Transient_Class_Not_Registered_Should_Throw()
        {
            try
            {
                container.GetService<BasicClass>();
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

        [Test]
        public void Getting_Transient_By_Class_With_Factory_Should_Return_Correct_Object()
        {
            container.RegisterTransient<BasicClass>(() => new DerivedClass());
            var transient = container.GetService<BasicClass>();
            Assert.That(transient.GetType() == typeof(DerivedClass));
        }

        [Test]
        public void Getting_Transient_By_Interface_With_Factory_Should_Return_Correct_Object()
        {
            container.RegisterTransient<ISomeInterface, SomeClassWithInterface>(() => new SomeDerivedClassWithInterface());
            var transient = container.GetService<ISomeInterface>();
            Assert.That(transient.GetType() == typeof(SomeDerivedClassWithInterface));
        }

        #endregion

        #region Getting via Types

        [Test]
        public void Getting_Class_By_Type_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(BasicClass));
            container.GetService(typeof(BasicClass));
            Assert.That(true);
        }

        [Test]
        public void Getting_Instance_By_Type_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(ISomeInterface), typeof(SomeClassWithInterface));
            container.GetService(typeof(ISomeInterface));
            Assert.That(true);
        }

        [Test]
        public void Getting_Class_By_Type_With_Factory_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(BasicClass), () => new BasicClass());
            container.GetService(typeof(BasicClass));
            Assert.That(true);
        }

        [Test]
        public void Getting_Instance_By_Type_With_Factory_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(ISomeInterface), typeof(SomeClassWithInterface), () => new SomeDerivedClassWithInterface());
            container.GetService(typeof(ISomeInterface));
            Assert.That(true);
        }

        [Test]
        public void Getting_Class_By_Type_With_Invalid_Factory_Should_Throw()
        {
            container.RegisterSingleton(typeof(BasicClass), () => new SomeOtherBasicClass());
            try
            {
                container.GetService(typeof(BasicClass));
                Assert.That(false);
            }
            catch (InvalidCastException)
            {
                Assert.That(true);
            }
            catch (Exception)
            {
                Assert.That(false);
            }
        }

        [Test]
        public void Getting_Instance_By_Type_With_Invalid_Factory_Should_Throw()
        {
            container.RegisterSingleton(typeof(ISomeInterface), typeof(SomeClassWithInterface), () => new BasicClass());
            try
            {
                container.GetService(typeof(ISomeInterface));
                Assert.That(false);
            }
            catch (InvalidCastException)
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
