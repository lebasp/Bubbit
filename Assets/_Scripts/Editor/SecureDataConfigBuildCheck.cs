using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Warns at build time when the SecureData config is missing, so a build never ships
// with the development-only fallback passphrase by accident.
class SecureDataConfigBuildCheck : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        if (!SecureDataConfig.Exists())
            Debug.LogWarning($"[SecureData] Building without {SecureDataConfig.AssetPath}. The build will use the development-only fallback passphrase. Copy {SecureDataConfig.ExamplePath} to {SecureDataConfig.AssetPath} first.");
    }
}
