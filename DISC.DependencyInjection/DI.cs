using System;

namespace DISC
{
    public static class DI
    {
        /// <summary>
        /// Creates a new root scope for depdency injection for the application.
        /// </summary>
        /// <returns><see cref="IRootDIScope"/> instance for registering services against.</returns>
        /// <exception cref="NotSupportedException">Thrown when Root Scope already exists.</exception>
        public static IRootDIScope CreateRootScope()
        {
            if (DIScope.RootScope != null)
            {
                throw new NotSupportedException("Main Scope already created.");
            }

            DIScope.RootScope = new RootDIScope();

            return DIScope.RootScope;
        }

        /// <summary>
        /// For resetting the root scope between tests.
        /// </summary>
        internal static void ClearMainScope()
        {
            RootDIScope.entryPointType = null;
            DIScope.RootScope = null;
        }
    }
}
