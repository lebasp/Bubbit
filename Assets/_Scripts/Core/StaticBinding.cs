/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class StaticBinding : MonoBehaviour
{
	static Dictionary<string,Object> dictionary = new Dictionary<string, Object>();

	public static T Get<T>() where T : Component
	{
		var key = typeof(T).ToString();

		if(dictionary.ContainsKey(key))
			return (T) dictionary[key];

		dictionary[key] = new GameObject(key).AddComponent<T>();

		return (T) dictionary[key];
	}

}
*/