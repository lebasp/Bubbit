using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class Frost : PowerUp {

	List<Circle> circles = new List<Circle>();

    bool m_moreThan5 = false;


	public override void PickUp ()
	{
		base.PickUp();

        m_backgroundController.Paint(Color);
        m_backgroundController.Pause(true);

		circles = m_circleSpawner.Circles;

        foreach(var circle in circles)
        {
            circle.FrozenState();
        }

        m_moreThan5 = circles.Count >= 7;

		StartCoroutine(Frozen());
	}
	
	IEnumerator Frozen()
	{			
		var timeNow = new TimeSpan (0);
		var endtime = new TimeSpan(0,0,5);

		while (timeNow < endtime)
		{
			if (!m_pause) 
			{
				var deltaTime = new TimeSpan (0, 0, 0, 0, (int)(1000 * Time.fixedDeltaTime));
				timeNow = timeNow.Add (deltaTime);

                var found = false;

                foreach (var circle in circles)
                {
                    if (circle.State == Circle.state.Frozen)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (m_moreThan5)
                        GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_let_it_go);

                    break;            
                }
            }

            yield return new WaitForFixedUpdate ();
		}

        ResetEffects();

        ResetPowerUp();
	}

    public void ResetEffects()
    {
        foreach (var circle in circles)
        {
            if (circle.State == Circle.state.Frozen)
                circle.FreeState();
        }

        m_backgroundController.Unpaint();
        m_backgroundController.Pause(false);
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
