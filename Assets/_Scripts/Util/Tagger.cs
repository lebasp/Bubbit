using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tagger : MonoBehaviour 
{	
	public List<string> Tags = new List<string>();
}


static class TaggerExtension
{
	public static bool ContainsTag(this GameObject go, string tag)
	{
		var tagger = go.GetComponent<Tagger> ();

		if (tagger != null && tagger.Tags.Contains(tag)) 
		{
			return true;
		}

        return false;
	}
}

