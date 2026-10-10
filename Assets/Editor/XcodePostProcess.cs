#if UNITY_IPHONE
using UnityEngine;
using System.Collections;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;
using UnityEditor;

public class XcodePostProcess
{

	[PostProcessBuild(101)]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
		if (buildTarget == BuildTarget.iOS) 
		{
			//disable bitcode
			string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
			PBXProject proj = new PBXProject ();

			proj.ReadFromString (File.ReadAllText (projPath));

			string target = proj.TargetGuidByName ("Unity-iPhone");

			proj.SetBuildProperty (target, "ENABLE_BITCODE", "false");

			File.WriteAllText (projPath, proj.WriteToString ());




			//set background notification on
			string plistPath = path + "/Info.plist";

			PlistDocument plist = new PlistDocument ();
			plist.ReadFromString (File.ReadAllText (plistPath));

			PlistElementDict root = plist.root;

			var buildKey = "UIBackgroundModes";
			root.CreateArray (buildKey).AddString ("remote-notification");

			File.WriteAllText (plistPath, plist.WriteToString ());
		}
	}

}
#endif