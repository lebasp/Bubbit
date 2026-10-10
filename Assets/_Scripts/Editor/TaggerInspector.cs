using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Tagger))]
public class TaggerInspector : Editor 
{	

	public override void OnInspectorGUI()
	{
		Tagger myTarget = (Tagger)target;

		var tags = UnityEditorInternal.InternalEditorUtility.tags;

		EditorGUILayout.BeginVertical ();

		foreach (var tag in tags)
		{		
			if (GUILayout.Toggle (myTarget.Tags.Contains (tag), tag)) 
			{
				if (!myTarget.Tags.Contains (tag))
					myTarget.Tags.Add (tag);
			}
			else if (myTarget.Tags.Contains (tag))
				myTarget.Tags.Remove (tag);					
				
		}

		EditorGUILayout.EndVertical ();
	}
}
