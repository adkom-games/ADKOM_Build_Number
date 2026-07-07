using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using System.IO;

namespace ADKOM
{
    /// <summary>
    /// Automatically increments the build number after each successful compilation.
    /// The asset lives under a Resources/ folder so the runtime can load it via Resources.Load.
    /// </summary>
    [InitializeOnLoad]
    public static class BuildNumberAutoIncrement
    {
        private const string ASSET_PATH = "Assets/Settings/Resources/BuildNumberData.asset";
        private const string PREFS_KEY_VERBOSE = "ADKOM_VerboseLogging";

        static BuildNumberAutoIncrement()
        {
            CompilationPipeline.compilationFinished += OnCompilationFinished;
        }

        private static void OnCompilationFinished(object obj)
        {
            if (EditorUtility.scriptCompilationFailed)
                return;

            var data = GetOrCreateBuildNumberData();
            if (data != null)
            {
                data.Increment();
                EditorUtility.SetDirty(data);
                AssetDatabase.SaveAssetIfDirty(data);
                if (VerboseLogging)
                    Debug.Log($"[ADKOM] Build number incremented to {data.BuildNumber}");
            }
        }

        /// <summary>
        /// Gets or sets whether verbose logging is enabled.
        /// </summary>
        public static bool VerboseLogging
        {
            get => EditorPrefs.GetBool(PREFS_KEY_VERBOSE, false);
            set => EditorPrefs.SetBool(PREFS_KEY_VERBOSE, value);
        }

        /// <summary>
        /// Editor-side accessor that uses AssetDatabase. Runtime code should prefer
        /// <see cref="BuildNumberData.CurrentBuildNumber"/> which works in both Editor and player.
        /// </summary>
        public static int GetBuildNumber()
        {
            var data = AssetDatabase.LoadAssetAtPath<BuildNumberData>(ASSET_PATH);
            return data != null ? data.BuildNumber : 0;
        }

        private static BuildNumberData GetOrCreateBuildNumberData()
        {
            var data = AssetDatabase.LoadAssetAtPath<BuildNumberData>(ASSET_PATH);

            if (data == null)
            {
                string directory = Path.GetDirectoryName(ASSET_PATH);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                data = ScriptableObject.CreateInstance<BuildNumberData>();
                AssetDatabase.CreateAsset(data, ASSET_PATH);
                AssetDatabase.SaveAssets();
                if (VerboseLogging)
                    Debug.Log($"[ADKOM] Created build number asset at {ASSET_PATH}");
            }

            return data;
        }
    }
}
