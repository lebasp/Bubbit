using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class DoublePoints : PowerUp {

    List<Circle> circles = new List<Circle>();

    int m_pointsMultiplier = 2;

    public override void PickUp ()
	{
		base.PickUp();

        m_backgroundController.Paint(Color);

        circles = m_circleSpawner.Circles;

        foreach (var circle in circles)
        {
            circle.SetDoublePointsEffect(true);
            circle.UpdatePointsTo(circle.Points * m_pointsMultiplier);
        }

        StartCoroutine(Double());
	}
	
	IEnumerator Double()
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

            yield return new WaitForFixedUpdate();
		}

        ResetEffects();

        ResetPowerUp();
	}

    void ResetEffects()
    {
        foreach (var circle in circles)
        {
            circle.UpdatePointsTo(circle.Points / m_pointsMultiplier);
            circle.SetDoublePointsEffect(false);
        }

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
