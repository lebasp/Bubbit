using UnityEngine;
using System.Collections;

public class IncreaseTime : PowerUp
{
    public override void PickUp ()
    {
        base.PickUp();

        m_gameManager.Gametimer.AddTime(10);

        ResetPowerUp();
    }
}
