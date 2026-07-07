using UnityEngine;

namespace ADKOM
{
    /// <summary>
    /// The single public entry point for reading the project's build number.
    ///
    /// The value is stored as a plain-text resource at
    /// <c>Assets/Settings/Resources/BuildNumber.txt</c> (always text, never binary,
    /// so it never needs Git LFS) and is incremented automatically after each
    /// successful compilation by the editor-only BuildNumberAutoIncrement.
    /// </summary>
    public static class BuildNumber
    {
        /// <summary>
        /// Resources-relative name of the backing text asset (no extension).
        /// Shared with the editor writer so the name has a single source of truth.
        /// </summary>
        public const string ResourceName = "BuildNumber";

        private static int? _cached;

        /// <summary>
        /// The current build number, or <c>0</c> if the asset has not been created yet.
        /// Works in the Editor, in Play mode, and in built players.
        /// </summary>
        public static int Current
        {
            get
            {
                // Static state resets on each domain reload, which in the Editor
                // coincides with the recompile that changes the value — so caching
                // is safe, and in a player the value is fixed for the session.
                if (_cached.HasValue)
                    return _cached.Value;

                var asset = Resources.Load<TextAsset>(ResourceName);
                int value = 0;
                if (asset != null)
                    int.TryParse(asset.text.Trim(), out value);

                _cached = value;
                return value;
            }
        }
    }
}
