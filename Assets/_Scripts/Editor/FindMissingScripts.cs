using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FindMissingScripts : EditorWindow 
{
        static int go_count = 0, components_count = 0, missing_count = 0;

        [MenuItem("Window/Find Missing Scripts (All)")]
        static void FindInAll()
        {
            go_count = 0;
            components_count = 0;
            missing_count = 0;
            foreach (var root in SceneRoots()) 
            {
                //Debug.Log(root);
                FindInGO(root);
            }
            Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
        }

        static void FindInGO(GameObject g)
        {
            go_count++;
            Component[] components = g.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                components_count++;
                if (components[i] == null)
                {
                    missing_count++;
                    string s = g.name;
                    Transform t = g.transform;
                    while (t.parent != null) 
                    {
                        s = t.parent.name +"/"+s;
                        t = t.parent;
                    }
                    Debug.Log (s + " has an empty script attached in position: " + i, g);
                }
            }
            // Now recurse through each child GO (if there are any):
            foreach (Transform childT in g.transform)
            {
                //Debug.Log("Searching " + childT.name  + " " );
                FindInGO(childT.gameObject);
            }
        }

        static IEnumerable<GameObject> SceneRoots()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                    continue;

                foreach (var root in scene.GetRootGameObjects())
                    yield return root;
            }
        }
}

