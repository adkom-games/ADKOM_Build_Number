using UnityEngine;

namespace ADKOM
{
    /// <summary>
    /// Stores the project build number. Automatically incremented on each compilation.
    /// The asset lives at Assets/Settings/Resources/BuildNumberData.asset so it can be
    /// loaded at runtime via Resources.Load — works in Editor and in built players.
    /// </summary>
    public class BuildNumberData : ScriptableObject
    {
        /// <summary>Resources-relative path used by <see cref="GetCurrent"/>.</summary>
        public const string ResourcesPath = "BuildNumberData";

        [SerializeField]
        private int buildNumber = 0;

        /// <summary>
        /// The current build number.
        /// </summary>
        public int BuildNumber => buildNumber;

        /// <summary>
        /// Increments the build number. Called automatically by BuildNumberAutoIncrement.
        /// </summary>
        public void Increment()
        {
            buildNumber++;
        }

        private static BuildNumberData _cached;

        /// <summary>
        /// Loads (and caches) the project's BuildNumberData asset via Resources.
        /// Works in both Editor (Play mode included) and built players.
        /// Returns null if the asset is missing from any Resources folder.
        /// </summary>
        public static BuildNumberData GetCurrent()
        {
            if (_cached != null) return _cached;
            _cached = Resources.Load<BuildNumberData>(ResourcesPath);
            return _cached;
        }

        /// <summary>
        /// Convenience: current build number, or 0 if no asset is found.
        /// </summary>
        public static int CurrentBuildNumber
        {
            get
            {
                var data = GetCurrent();
                return data != null ? data.BuildNumber : 0;
            }
        }
    }
}
