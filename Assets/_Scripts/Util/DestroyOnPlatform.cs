using UnityEngine;
using System.Collections;

public class DestroyOnPlatform : MonoBehaviour {

    [SerializeField]
    RuntimePlatform m_platform;

	// Use this for initialization
	void Awake () 
    {
        if (Application.platform == m_platform)
            Destroy(this.gameObject);
	}
}
