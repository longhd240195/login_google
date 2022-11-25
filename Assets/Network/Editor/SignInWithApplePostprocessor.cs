using AppleAuth.Editor;

using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor;

public static class SignInWithApplePostprocessor
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
#if UNITY_IOS
        if (target != BuildTarget.iOS)
            return;

        var projectPath = PBXProject.GetPBXProjectPath(path);

        // Adds entitlement depending on the Unity version used
#if UNITY_2019_3_OR_NEWER
        var project = new PBXProject();
        project.ReadFromString(System.IO.File.ReadAllText(projectPath));
        var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, project.GetUnityMainTargetGuid());
        manager.AddSignInWithAppleWithCompatibility(project.GetUnityFrameworkTargetGuid());
        manager.WriteToFile();
#else
            var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", PBXProject.GetUnityTargetName());
            manager.AddSignInWithAppleWithCompatibility();
            manager.WriteToFile();
#endif
#endif
    }
}