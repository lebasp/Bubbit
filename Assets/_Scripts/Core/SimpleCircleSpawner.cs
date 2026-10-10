using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class SimpleCircleSpawner : MonoBehaviour 
{
    BackgroundController m_backgroundController { get { return BackgroundController.GetInstance(); } }

    [SerializeField]
    int m_spawnMS;

	[SerializeField]
	GameObject m_circlePrefab;

	[SerializeField]
    List<SimpleCircle> m_circlePreList;

    Pool<SimpleCircle> m_circlePool;

	[SerializeField]
	RectTransform m_spawnArea;

	Vector2 m_spawnPointStart;
	Vector2 m_spawnPointEnd;

    bool m_paused = false;

	void Awake ()
	{
        m_circlePool = Pool<SimpleCircle>.CreatePool(m_circlePreList, m_circlePrefab, transform);

        Invoke("StartSpawn", 1);
	}

	public void StartSpawn()
	{
		var y = m_spawnArea.transform.localPosition.y + m_spawnArea.rect.yMax;

		m_spawnPointStart = new Vector2( m_spawnArea.rect.xMin, y);
		m_spawnPointEnd = new Vector2(m_spawnArea.rect.xMax,  y);

		StartCoroutine(Spawner());
	}

	IEnumerator Spawner()
	{
		yield return null;

		var nextSpawnTime = new TimeSpan (0);
		var timeNow = new TimeSpan (0);

		float previousX = 0;


        while (true) 
        {           
            if (!m_paused)
            {
                var deltaTime = new TimeSpan(0, 0, 0, 0, (int)((1000 * Time.fixedDeltaTime)));
                timeNow = timeNow.Add(deltaTime);

                if (timeNow >= nextSpawnTime)
                {
                    float x = 0;

                    do
                    {
                        x = Random.Range(m_spawnPointStart.x, m_spawnPointEnd.x);
                    } while(x >= previousX - 100 && x <= previousX + 100) ;

                    previousX = x;

                    var y = Random.Range(m_spawnPointStart.y, m_spawnPointEnd.y);

                    var spawnPosition = new Vector3(x, y);

                    SpawnCircleAtLocation(spawnPosition);			
                      
                    nextSpawnTime = timeNow.Add(new TimeSpan(0, 0, 0, 0, m_spawnMS));
                }   
            }

            yield return new WaitForFixedUpdate ();
        }
	}

	void SpawnCircleAtLocation(Vector3 spawnPosition)
	{
		var circle = m_circlePool.GetObject();

		circle.Setup (spawnPosition, this);

        m_backgroundController.AddActiveCircle(circle);
	}

    public void Remove(SimpleCircle circle)
    {
        m_circlePool.DeactivateFromPool(circle);

        m_backgroundController.RemoveActiveCircle(circle);
    }

    public void Pause(bool pause)
    {
        m_paused = pause;
    }

}
