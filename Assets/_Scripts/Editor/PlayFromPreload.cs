using UnityEditor;
using UnityEditor.SceneManagement;

// Always enter Play Mode through the preload scene, which creates the
// DontDestroyOnLoad managers (TransitionEffect, BackgroundController, etc.)
// that the menu and gameplay scenes depend on.
[InitializeOnLoad]
static class PlayFromPreload
{
    const string PreloadScenePath = "Assets/Scene/preload.unity";

    static PlayFromPreload()
    {
        EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(PreloadScenePath);
    }
}
