using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace ADKOM
{
    /// <summary>
    /// Automatically increments the build number after each successful compilation.
    /// The value is stored as a plain-text file under a Resources/ folder so the
    /// runtime can read it via <see cref="BuildNumber.Current"/> in the Editor, in
    /// Play mode, and in built players. Being plain text, it never needs Git LFS and
    /// is unaffected by the project's asset serialization mode.
    /// </summary>
    [InitializeOnLoad]
    public static class BuildNumberAutoIncrement
    {
        /// <summary>Project-relative path of the backing text asset.</summary>
        private static readonly string AssetPath =
            $"Assets/Settings/Resources/{BuildNumber.ResourceName}.txt";

        private const string PrefsKeyVerbose = "ADKOM_VerboseLogging";

        static BuildNumberAutoIncrement()
        {
            CompilationPipeline.compilationFinished += OnCompilationFinished;
        }

        /// <summary>
        /// Gets or sets whether each increment is logged to the Console.
        /// </summary>
        public static bool VerboseLogging
        {
            get => EditorPrefs.GetBool(PrefsKeyVerbose, false);
            set => EditorPrefs.SetBool(PrefsKeyVerbose, value);
        }

        private static void OnCompilationFinished(object context)
        {
            if (EditorUtility.scriptCompilationFailed)
                return;

            int next = ReadCurrent() + 1;
            Write(next);

            if (VerboseLogging)
                Debug.Log($"[ADKOM] Build number incremented to {next}");
        }

        /// <summary>Reads the current value from the backing file, or 0 if absent.</summary>
        internal static int ReadCurrent()
        {
            if (File.Exists(AssetPath) &&
                int.TryParse(File.ReadAllText(AssetPath).Trim(), out int value))
            {
                return value;
            }

            return 0;
        }

        /// <summary>Writes the value to the backing file, creating it if needed.</summary>
        internal static void Write(int value)
        {
            string directory = Path.GetDirectoryName(AssetPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(AssetPath, value.ToString());
            AssetDatabase.ImportAsset(AssetPath, ImportAssetOptions.ForceUpdate);
        }
    }
}
