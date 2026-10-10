using UnityEngine;
using System.Collections;

public class RelativeTop : MonoBehaviour
{
    [SerializeField]
    RectTransform m_rect;
    [SerializeField]
    float space;

    IEnumerator Start()
    {
        yield return null;
            
        var myRect = GetComponent<RectTransform>();

        myRect.offsetMax = new Vector2(myRect.offsetMax.x, -(m_rect.rect.height + space));
    }

}
