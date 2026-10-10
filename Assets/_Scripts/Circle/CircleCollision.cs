using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleCollision : MonoBehaviour
{
	#region STATICS
	CanvasSettings m_canvasSettings {get { return CanvasSettings.GetInstance(); } }
	CircleSpawner m_circleSpawner {get { return CircleSpawner.GetInstance(); } }
	GameStats m_gameStats {get { return GameStats.GetInstance(); } }
	HealthManager m_healthManager { get { return HealthManager.GetInstance (); } }
    GameManager m_gameManager { get { return GameManager.GetInstance(); } }
    #endregion

    static CircleCollision s_instance;
	public static CircleCollision GetInstance(){ return s_instance; }

    List<Circle> collisions = new List<Circle>();

	[SerializeField]
	AudioSource m_source;

    void Awake () 
	{
		if(!s_instance) s_instance = this;
	}

	public void CollideCircles(Circle circle1, Circle circle2)
	{
        var circle1higher = circle1.Points >= circle2.Points;
        var circle1Locked = circle1.State == Circle.state.Controlling || circle1.State == Circle.state.Locked;
        var circle2Locked = circle2.State == Circle.state.Controlling || circle2.State == Circle.state.Locked;
            

        var highCircle = (circle1Locked ||  (circle1higher && !circle2Locked))? circle1 : circle2;
        var lowCircle  = (circle1Locked ||  (circle1higher && !circle2Locked))? circle2 : circle1;

		InternalCollideCircles(highCircle, lowCircle);
	}

	void InternalCollideCircles(Circle highCircle, Circle lowCircle)
	{
        if (collisions.Contains(highCircle) || collisions.Contains(lowCircle))
            return;

        highCircle.DisableCollider();
        lowCircle.DisableCollider();

		collisions.Add(highCircle);
        collisions.Add(lowCircle);

        StartCoroutine(CollisionAnimation(highCircle, lowCircle));
	}

	IEnumerator CollisionAnimation(Circle highCircle, Circle lowCircle)
	{	
		yield return null;
		
        var highDeath = highCircle.gameObject.ContainsTag ("death");
		var lowDeath = lowCircle.gameObject.ContainsTag ("death");

		if ( !((highDeath && !lowDeath) || (!highDeath && lowDeath)) )
		{			
			m_source.Play ();
		}

		while (true) 
		{
			var distance = Vector3.Distance (highCircle.Position, lowCircle.Position);		
			var normalize = Vector3.Normalize (highCircle.Position - lowCircle.Position);					
			
			var collisionSpeed = lowCircle.Speed;

			lowCircle.SetPosition (lowCircle.Position + (normalize * collisionSpeed));

			if (distance <= (collisionSpeed))
				break;
			
			yield return new WaitForFixedUpdate ();
		}

        var explode = false;

        if (highDeath || lowDeath)
        {			
            if (!highDeath || !lowDeath)
                m_healthManager.RemoveLife();
            else
                GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_killing_what_kills_you);
        }
        else
        {	
            var spawnPoint = new Vector2(m_canvasSettings.Convert(highCircle.Position.x), m_canvasSettings.Convert(highCircle.Position.y));
            var points = highCircle.Points + lowCircle.Points;

            explode = CalculatePoints(spawnPoint, points);

            lowCircle.UserPoweUp();
            highCircle.UserPoweUp();			
        }

        if(highCircle.Points >= 18 || lowCircle.Points >= 18)
            GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_overscored_bubble);


        collisions.Remove (highCircle);
        collisions.Remove (lowCircle);  

		highCircle.KillCircle();
		lowCircle.KillCircle();     

        if (explode)
            m_gameManager.Explode();
	}

	bool CalculatePoints(Vector2 spawnPoint, int points)
	{   
        FloatingScoreController.GetInstance().ShowPoints(spawnPoint, points);
        m_gameStats.AddScore(points);
        
        if (points > 0 && points < 10)
        {
            m_circleSpawner.SpawnCircleAtLocation(spawnPoint, points, true);
            return false;
        }

        return true;
    }
}
