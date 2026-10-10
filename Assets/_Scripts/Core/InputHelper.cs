using UnityEngine;
using System.Collections;

public class InputHelper
{
	public static Camera MainCamera;

	public static Vector3 MousePosition()
	{
		if(!MainCamera)
			MainCamera = Camera.main;

		return MainCamera.ScreenToWorldPoint(Input.mousePosition);

	}
}
