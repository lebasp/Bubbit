using System;
using UnityEngine;

// Loads the SecureData passphrase from Assets/Resources/SecureDataConfig.json, which is
// kept out of Git. Resources works the same on Android, iOS and WebGL.
// Setup: copy Config/SecureDataConfig.example.json to Assets/Resources/SecureDataConfig.json
// and set your own passphrase.
public static class SecureDataConfig
{
    public const string ResourcePath = "SecureDataConfig";
    public const string AssetPath = "Assets/Resources/SecureDataConfig.json";
    public const string ExamplePath = "Config/SecureDataConfig.example.json";

    // Development-only fallback so the game still runs without the config file.
    // Saves written with it are not compatible with builds that use the real passphrase.
    const string DevelopmentFallbackPassphrase = "bubbit-development-only";

    [Serializable]
    class Data
    {
        public string passphrase;
    }

    static string s_passphrase;

    public static string Passphrase
    {
        get
        {
            if (s_passphrase == null)
                s_passphrase = Load();

            return s_passphrase;
        }
    }

    public static bool Exists()
    {
        return Resources.Load<TextAsset>(ResourcePath) != null;
    }

    static string Load()
    {
        var file = Resources.Load<TextAsset>(ResourcePath);

        if (file != null)
        {
            var data = JsonUtility.FromJson<Data>(file.text);
            if (data != null && !string.IsNullOrEmpty(data.passphrase))
                return data.passphrase;

            Debug.LogError($"[SecureData] {AssetPath} has no \"passphrase\" value. Using the development-only fallback passphrase.");
        }
        else
        {
            var message = $"[SecureData] Config file not found. Copy {ExamplePath} to {AssetPath} and set your own passphrase. " +
                          "Using the development-only fallback passphrase for now.";
#if UNITY_EDITOR
            Debug.LogWarning(message);
#else
            Debug.LogError(message);
#endif
        }

        return DevelopmentFallbackPassphrase;
    }
}
