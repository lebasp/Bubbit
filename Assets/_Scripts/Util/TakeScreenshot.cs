using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class TakeScreenshot : MonoBehaviour 
{
	public Camera m_watermark;

    public void Screenshot(string text, DisposableAction action)
    {
        StartCoroutine(ProcessScreeShot(text, action));   
    }

    IEnumerator ProcessScreeShot(string text, DisposableAction action)
    {
		if (m_watermark != null)
			m_watermark.enabled = true;

        yield return new WaitForEndOfFrame(); 


		var image = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);         


		image.ReadPixels(new Rect(0,0, Screen.width, Screen.height), 0, 0, false);
        image.Apply();

		if (m_watermark != null)
			m_watermark.enabled = false;

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (!Application.isEditor)
        {

            byte[] dataToSave = image.EncodeToPNG();

            string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
            File.WriteAllBytes(destination, dataToSave);

            NativeShare.Share(text + GameConstants.StoreLink, destination, GameConstants.StoreLink, "Bubbit!");

        }
        */

		if (action != null)
			action.Run ();
    }
}
