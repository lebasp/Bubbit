using UnityEngine;
using System.Collections;

public abstract class ITutorialStep : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    public abstract string Text{ get; }
    public abstract string Tip{ get; }
    public abstract void Enable();
    public abstract void Disable();

}
