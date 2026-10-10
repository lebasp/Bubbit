using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Command-line WebGL build. Builds the enabled EditorBuildSettings scenes to Build/WebGL:
// Unity.exe -batchmode -quit -projectPath . -buildTarget WebGL -executeMethod WebGLBuild.Build -logFile build.log
static class WebGLBuild
{
    const string OutputPath = "Build/WebGL";

    [MenuItem("Build/WebGL")]
    public static void Build()
    {
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = OutputPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;
        Debug.Log($"[WebGLBuild] {summary.result}: {summary.totalErrors} errors, {summary.totalSize} bytes, {summary.totalTime}");

        if (Application.isBatchMode && summary.result != BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }
}
