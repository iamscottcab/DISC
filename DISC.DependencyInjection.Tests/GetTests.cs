using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
            DI.ClearRootScope();
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

        #region Scoped

        [Test]
        public void Getting_Scoped_Class_Not_Registered_Should_Throw()
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
        public void Getting_Scoped_Class_Registered_Should_Not_Throw()
        {
            container.RegisterScoped<BasicClass>();

            var scope = container.GetService<IScopeProvider>().CreateScope();
            scope.GetService<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Scoped_Of_Same_Class_Should_Not_Throw()
        {
            container.RegisterScoped<BasicClass>();

            var scope = container.GetService<IScopeProvider>().CreateScope();
            scope.GetService<BasicClass>();
            scope.GetService<BasicClass>();

            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Scoped_Of_Same_Class_Resolve_Same_Object()
        {
            container.RegisterScoped<BasicClass>();

            var scope = container.GetService<IScopeProvider>().CreateScope();
            var scopedOne = scope.GetService<BasicClass>();
            var scopedTwo = scope.GetService<BasicClass>();

            Assert.AreEqual(scopedOne, scopedTwo);
        }

        [Test]
        public void Getting_Multiple_Scoped_Of_Same_Class_Across_Scopes_Resolve_Different_Object()
        {
            container.RegisterScoped<BasicClass>();
            
            var scopeProvider = container.GetService<IScopeProvider>();
            var scopeOne = scopeProvider.CreateScope();
            var scopeTwo = scopeProvider.CreateScope();

            var scopedOne = scopeOne.GetService<BasicClass>();
            var scopedTwo = scopeTwo.GetService<BasicClass>();

            Assert.AreNotEqual(scopedOne, scopedTwo);
        }

        [Test]
        public void Getting_Scoped_Interface_Not_Registered_Should_Throw()
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
        public void Getting_Scoped_Interface_Registered_Should_Not_Throw()
        {
            container.RegisterScoped<ISomeInterface, SomeClassWithInterface>();
            
            var scope = container.GetService<IScopeProvider>().CreateScope();
            scope.GetService<ISomeInterface>();

            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Scoped_Of_Same_Interface_Should_Not_Throw()
        {
            container.RegisterScoped<ISomeInterface, SomeClassWithInterface>();
            
            var scope = container.GetService<IScopeProvider>().CreateScope();
            scope.GetService<ISomeInterface>();
            scope.GetService<ISomeInterface>();

            Assert.That(true);
        }

        [Test]
        public void Getting_Multiple_Scoped_Of_Same_Interface_Resolve_Same_Object()
        {
            container.RegisterScoped<ISomeInterface, SomeClassWithInterface>();

            var scope = container.GetService<IScopeProvider>().CreateScope();
            var scopedOne = scope.GetService<ISomeInterface>();
            var scopedTwo = scope.GetService<ISomeInterface>();

            Assert.AreEqual(scopedOne, scopedTwo);
        }

        [Test]
        public void Getting_Multiple_Scoped_Of_Same_Interface_Across_Scopes_Resolve_Different_Object()
        {
            container.RegisterScoped<ISomeInterface, SomeClassWithInterface>();
  
            var scopeProvider = container.GetService<IScopeProvider>();
            var scopeOne = scopeProvider.CreateScope();
            var scopeTwo = scopeProvider.CreateScope();
            
            var scopedOne = scopeOne.GetService<ISomeInterface>();
            var scopedTwo = scopeTwo.GetService<ISomeInterface>();

            Assert.AreNotEqual(scopedOne, scopedTwo);
        }

        [Test]
        public void Getting_Scoped_By_Class_With_Factory_Should_Return_Correct_Object()
        {
            container.RegisterScoped<BasicClass>(() => new DerivedClass());
    
            var scope = container.GetService<IScopeProvider>().CreateScope();
            var scoped = scope.GetService<BasicClass>();

            Assert.That(scoped.GetType() == typeof(DerivedClass));
        }

        [Test]
        public void Getting_Scoped_By_Interface_With_Factory_Should_Return_Correct_Object()
        {
            container.RegisterScoped<ISomeInterface, SomeClassWithInterface>(() => new SomeDerivedClassWithInterface());

            var scope = container.GetService<IScopeProvider>().CreateScope();
            var scoped = scope.GetService<ISomeInterface>();

            Assert.That(scoped.GetType() == typeof(SomeDerivedClassWithInterface));
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

        #region Open Generics

        [Test]
        public void Getting_Concrete_Generic_Should_Not_Throw()
        {
            container.RegisterSingleton(typeof(IGenericInterface<>), typeof(GenericClassWithInterface<>));
            container.RegisterSingleton<BasicClass>();
            var service = container.GetService<IGenericInterface<BasicClass>>();
            Assert.IsNotNull(service);
            Assert.IsAssignableFrom<GenericClassWithInterface<BasicClass>>(service);
        }

        [Test]
        public void Getting_Concrete_Generic_Singleton_From_Scope_Should_Resolve_Same_Object_In_Root()
        {
            container.RegisterSingleton(typeof(IGenericInterface<>), typeof(GenericClassWithInterface<>));
            container.RegisterSingleton<BasicClass>();

            var scopeProvider = container.GetService<IScopeProvider>();
            
            var scopeOne = scopeProvider.CreateScope();
            var serviceOne = scopeOne.GetService<IGenericInterface<BasicClass>>();

            var serviceTwo = container.GetService<IGenericInterface<BasicClass>>();

            Assert.AreEqual(serviceOne, serviceTwo);
        }

        #endregion

        #region Lifetime Validation

        [Test]
        public void Getting_Captive_Dependency_By_Class_Should_Throw()
        {
            container.RegisterTransient<BasicClass>();
            container.RegisterSingleton<ClassWithDependency>();

            try
            {
                container.GetService<ClassWithDependency>();
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
        public void Getting_Captive_Dependency_By_Interface_Should_Throw()
        {
            container.RegisterTransient<BasicClass>();
            container.RegisterSingleton<IInterfaceWithDependency, ClassWithDependency>();

            try
            {
                container.GetService<IInterfaceWithDependency>();
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
        public void Getting_Captive_Dependency_Directly_Should_Not_Throw()
        {
            container.RegisterTransient<BasicClass>();
            container.RegisterSingleton<IInterfaceWithDependency, ClassWithDependency>();
            container.GetService<BasicClass>();
            Assert.That(true);
        }

        #endregion

        #region IEnumerables

        [Test]
        public void Getting_IEnumerable_Of_Class_Should_Not_Throw()
        {
            container.RegisterSingleton<BasicClass>();
            container.RegisterSingleton<BasicClass>(() => new DerivedClass());
            container.RegisterSingleton<ClassWithIEnumerableDependency<BasicClass>>();

            var result = container.GetService<ClassWithIEnumerableDependency<BasicClass>>();

            var collection = result.Classes.ToList();

            Assert.That(collection.Count == 2);
            Assert.That(collection[0].GetType() == typeof(BasicClass));
            Assert.That(collection[1].GetType() == typeof(DerivedClass));
        }

        [Test]
        public void Getting_IEnumerable_Of_Interface_Should_Not_Throw()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();
            container.RegisterSingleton<ISomeInterface, SomeOtherClassWithInterface>();
            container.RegisterSingleton<ClassWithIEnumerableDependency<ISomeInterface>>();

            var result = container.GetService<ClassWithIEnumerableDependency<ISomeInterface>>();

            var collection = result.Classes.ToList();

            Assert.That(collection.Count == 2);
            Assert.That(collection[0].GetType() == typeof(SomeClassWithInterface));
            Assert.That(collection[1].GetType() == typeof(SomeOtherClassWithInterface));
        }

        [Test]
        public void Getting_Services_From_Scope_Should_Return_Collection()
        {
            container.RegisterSingleton<ISomeInterface, SomeClassWithInterface>();
            container.RegisterSingleton<ISomeInterface, SomeOtherClassWithInterface>();

            var result = container.GetServices<ISomeInterface>();

            Assert.That(result is IEnumerable<ISomeInterface>);

            var resultAsList = result.ToList();

            Assert.That(resultAsList.Count == 2);
            Assert.That(resultAsList[0].GetType() == typeof(SomeClassWithInterface));
            Assert.That(resultAsList[1].GetType() == typeof(SomeOtherClassWithInterface));
        }

        #endregion
    }
}
