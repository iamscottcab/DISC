using System;

namespace DISC
{
    public interface IDIScope
    {
        /// <summary>
        /// Resolves the requested service by Type.
        /// Useful for dynamic runtime service resolution where generics are not available.
        /// </summary>
        /// <returns><see cref="object"/>Returns an object matching the type requested which can be hard cast to the relevant service.</returns>
        /// <exception cref="NullReferenceException">Thrown if the requested service cannot be found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the requested service does not have exactly 0 or 1 constructors.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the no service type is provided.</exception>
        /// <exception cref="NotSupportedException">Thrown if service lifetime validation fails.</exception>
        object GetService(Type serviceType);

        /// <summary>
        /// Resolves the requested service.
        /// </summary>
        /// <returns>The type of TService provided to the generic</returns>
        /// <exception cref="NullReferenceException">Thrown if the requested service cannot be found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the requested service does not have exactly 0 or 1 constructors.</exception>
        /// <exception cref="NotSupportedException">Thrown if service lifetime validation fails.</exception>
        TService GetService<TService>();
    }
}
