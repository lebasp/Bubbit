using UnityEngine;
using System.Collections;

public class TutorialStep5 : ITutorialStep
{
    public override string Text{ get { return Localization.CurrentLanguage.GetString("TUTORIAL_5"); } }
    public override string Tip{ get { return Localization.CurrentLanguage.GetString("PRO_TIP_1"); } }

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }
    
}
