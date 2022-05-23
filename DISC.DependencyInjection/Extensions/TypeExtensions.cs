using System;
using System.Linq;

namespace DISC
{
    static class TypeExtensions
    {
        internal static bool IsAssignableToOpenGenericType(this Type type, Type genericType)
        {
            if (type == null || genericType == null)
            {
                return false;
            }

            return type == genericType
              || type.MapsToGenericTypeDefinition(genericType)
              || type.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
              || (type.BaseType != null && type.BaseType.IsAssignableToOpenGenericType(genericType));
        }

        private static bool MapsToGenericTypeDefinition(this Type type, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
              && type.IsGenericType
              && type.GetGenericTypeDefinition() == genericType;
        }

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type type, Type genericType)
        {
            return type
              .GetInterfaces()
              .Where(it => it.IsGenericType)
              .Any(it => it.GetGenericTypeDefinition() == genericType);
        }
    }
}
