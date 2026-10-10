using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

// Command-line WebGL build. Builds the enabled EditorBuildSettings scenes to Build/WebGL:
// Unity.exe -batchmode -quit -projectPath . -buildTarget WebGL -executeMethod WebGLBuild.Build -logFile build.log
// BuildDevelopment does the same as a development build (logs, profiler) in Build/WebGLDev.
static class WebGLBuild
{
    const string OutputPath = "Build/WebGL";
    const string DevelopmentOutputPath = "Build/WebGLDev";

    [MenuItem("Build/WebGL")]
    public static void Build()
    {
        BuildTo(OutputPath, BuildOptions.None);
    }

    [MenuItem("Build/WebGL (Development)")]
    public static void BuildDevelopment()
    {
        BuildTo(DevelopmentOutputPath, BuildOptions.Development);
    }

    static void BuildTo(string outputPath, BuildOptions buildOptions)
    {
        string[] scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = BuildTarget.WebGL,
            options = buildOptions
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;
        Debug.Log($"[WebGLBuild] {summary.result}: {summary.totalErrors} errors, {summary.totalSize} bytes, {summary.totalTime}");

        if (Application.isBatchMode && summary.result != BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }
}
