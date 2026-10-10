using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockScreenColored : MonoBehaviour
{
    #region STATICS
    BackgroundController m_backgroundController { get { return BackgroundController.GetInstance(); } }
    #endregion

    Color m_defaultColor;

    Image m_image;

    public void Start()
    {
        m_image = GetComponent<Image>();
        m_defaultColor = m_image.color;

        ChangeColor(m_backgroundController.BackgroundColor());

        m_backgroundController.OnColorChanged += ChangeColor;
    }

    public void OnDestroy()
    {
        m_backgroundController.OnColorChanged -= ChangeColor;
    }

    void ChangeColor(Color color)
    {        
        var newColor = (m_defaultColor + color) * 0.5f;

        newColor = (m_defaultColor + newColor) * 0.5f;

        newColor = new Color(newColor.r, newColor.g, newColor.b, m_defaultColor.a);
        
        m_image.color = newColor;
    }

}
