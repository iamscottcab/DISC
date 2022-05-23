namespace DISC.Tests
{
    public class BasicClass { }
    public class DerivedClass : BasicClass { }
    public class SomeOtherBasicClass { }
    public abstract class BasicAbstractClass { }
    public interface ISomeInterface { }
    public class SomeClassWithInterface : ISomeInterface { }
    public class SomeDerivedClassWithInterface : SomeClassWithInterface { }
    public class SomeOtherClassWithInterface : ISomeInterface { }
    public class ClassWithConstructor
    {
        public ClassWithConstructor() { }
    }

    public class ClassWithMultipleConstructors
    {
        public ClassWithMultipleConstructors() { }

        public ClassWithMultipleConstructors(string s) { }
    }

    public interface IClassWithStringProperty
    {
        string Value { get; set; }
    }


    public class ClassWithStringProperty : IClassWithStringProperty
    {
        public string Value { get; set; }
    }
}
