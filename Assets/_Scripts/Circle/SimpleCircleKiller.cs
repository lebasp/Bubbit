using UnityEngine;
using System.Collections;

public class SimpleCircleKiller : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D coll)
    {
        var circle = coll.gameObject.GetComponent<SimpleCircle>();

        circle.KillCircle();
    }
}
