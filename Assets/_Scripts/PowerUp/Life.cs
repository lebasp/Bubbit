using UnityEngine;
using System.Collections;

public class Life : PowerUp
{
    public override void PickUp ()
    {
        base.PickUp();

        HealthManager.GetInstance().AddLife();

        ResetPowerUp();
    }
}
