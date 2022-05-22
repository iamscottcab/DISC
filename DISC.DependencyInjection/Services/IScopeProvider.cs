namespace DISC
{
    public interface IScopeProvider
    {
        /// <summary>
        /// Creates a new DI Scope to allow for DI of services registered as scoped.
        /// </summary>
        /// <returns>A new <see cref="IDIScope"/>with which to request services from.</returns>
        IDIScope CreateScope();
    }
}
