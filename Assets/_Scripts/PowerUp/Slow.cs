using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class Slow : PowerUp {

	public override void PickUp ()
	{
		base.PickUp();

        m_backgroundController.Paint(Color);

        m_gameManager.SpeedModifier *= 0.5f;

		StartCoroutine(SlowDown());
	}
	
	IEnumerator SlowDown()
	{
        

		var timeNow = new TimeSpan (0);
		var endtime = new TimeSpan(0,0,5);

		while (timeNow < endtime)
		{
			if (!m_pause) 
			{
				var deltaTime = new TimeSpan (0, 0, 0, 0, (int)(1000 * Time.fixedDeltaTime));
				timeNow = timeNow.Add (deltaTime);
            }

            yield return new WaitForFixedUpdate ();
		}

        ResetEffects();

        ResetPowerUp();
	}

    public void ResetEffects()
    {
        m_gameManager.SpeedModifier /= 0.5f;
        m_backgroundController.Unpaint();
    }

    public override void ForceStop()
    {
        base.ForceStop();

        if (m_running)
        {
            StopAllCoroutines();

            ResetEffects();
        }

        ResetPowerUp();
    }
}
