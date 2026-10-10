using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class BlackHole : PowerUp 
{
    GameStats m_gameStats { get { return GameStats.GetInstance(); } }

    List<Tween> tweens = new List<Tween>();

    float m_speed = 2;

    List<Circle> m_circlesRemaningInside = new List<Circle>();

    public override void PickUp ()
	{
		base.PickUp();

		m_spriteRenderer.enabled = true;

		tweens.Add( m_transform.DORotate(new Vector3(0,0, -360), 2.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart) );
		m_transform.DOScale(new Vector3(370,370), 0.2f).SetEase(Ease.Linear).OnComplete(() => 
		{

			tweens.Add( m_transform.DOScale(new Vector3(390,390), 0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear) );

		});

		StartCoroutine(PullForce());
	}

	IEnumerator PullForce()
	{
		var circles = m_circleSpawner.Circles;

		var timeNow = new TimeSpan (0);
		var endtime = new TimeSpan(0,0,3);

        var deathCircleList = new List<Circle>();

        do
        {
            yield return new WaitForFixedUpdate();

            if (!m_pause)
            {
                var deltaTime = new TimeSpan(0, 0, 0, 0, (int)(1000 * Time.fixedDeltaTime));
                timeNow = timeNow.Add(deltaTime);

                m_circlesRemaningInside.Clear();

                for (int i = circles.Count - 1; i >= 0; i--)
                {
                    circles[i].IsMoving = false;

                    if (circles[i].gameObject.ContainsTag("death"))
                        deathCircleList.Add(circles[i]);
                    else
                    {
                        var distance = Vector2.Distance(m_transform.position, circles[i].Position);
                        if (distance > m_speed)
                        {
                            var direction = (m_transform.position - circles[i].Position).normalized;

                            var newPosition = circles[i].Position + direction * m_speed;

                            circles[i].SetPosition(newPosition);
                        }
                        else
                        {
                            circles[i].SetPosition(m_transform.position);

                            if(!m_circlesRemaningInside.Contains(circles[i]))
                                m_circlesRemaningInside.Add(circles[i]);
                        }
                    }
                }

                if (deathCircleList.Count > 0)
                {
                    foreach (var circle in deathCircleList)
                    {
                        circle.KillCircle();
                    }

                    deathCircleList.Clear();
                }

            }
			
        } while (timeNow < endtime);

        for (int i = m_circlesRemaningInside.Count -1; i >= 0; i--) 
        {
            if(m_circlesRemaningInside[i] != null)
            {
                m_gameStats.AddScore(m_circlesRemaningInside[i].Points);
                m_circlesRemaningInside[i].UserPoweUp();
                m_circlesRemaningInside[i].KillCircle();
            }
        }

        m_circlesRemaningInside.Clear();

        ResetEffect();

        tweens.Add( m_transform.DOScale(new Vector3(0,0), 0.2f).SetEase(Ease.Linear).OnComplete(() => ResetPowerUp() ) );
	}

    public void ResetEffect()
    {
        if (m_running)
        {
            foreach (var tween in tweens)
                tween.Kill();

            tweens.Clear();

            foreach(var circle in m_circleSpawner.Circles)
            {
                circle.IsMoving = true;
            }
        }
    }

    public override void ForceStop()
    {
        base.ForceStop();

        if (m_running)
        {
            StopAllCoroutines();

            ResetEffect();
        }

        ResetPowerUp();
    }
}
