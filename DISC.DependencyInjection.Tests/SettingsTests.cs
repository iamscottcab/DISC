using NUnit.Framework;

namespace DISC.Tests
{
    class SettingsTests
    {
        [TearDown]
        public void AfterEach()
        {
            DI.ClearRootScope();
        }

        [Test]
        public void Overriding_Validation_Should_Allow_Captured_Dependencies()
        {
            var container = DI.CreateRootScope(new DISettings()
            {
                ValidateServiceLifetime = false
            });

            container.RegisterTransient<BasicClass>();
            container.RegisterSingleton<ClassWithDependency>();
            container.GetService<ClassWithDependency>();
            Assert.That(true);
        }

        [Test]
        public void Overriding_Scoped_In_Root_Should_Not_Throw()
        {
            var container = DI.CreateRootScope(new DISettings()
            {
                AllowScopedInRoot = true
            });

            container.RegisterScoped<BasicClass>();
            container.GetService<BasicClass>();
            Assert.That(true);
        }
    }
}
