namespace DISC
{
    /// <summary>
    /// A configuration class for changing the default DI behaviour
    /// </summary>
    public class DISettings
    {
        /// <summary>
        /// Whether or not captured dependencies should throw an error.
        /// Captured dependencies have a lifetime shorter than their parent and thus implicity take this lifetime.
        /// </summary>
        public bool ValidateServiceLifetime { get; set; } = true;

        /// <summary>
        /// Whether a service with a Scoped Lifetime should be allowed in the Root Scope.
        /// This is another form of liftime invalidation as they effectively become Singletons.
        /// </summary>
        public bool AllowScopedInRoot { get; set; } = false;
    }
}
