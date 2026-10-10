using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour 
{
	#region STATICS
	GameManager m_gameManager {get { return GameManager.GetInstance(); } }
	#endregion

	static HealthManager s_instance;
	public static HealthManager GetInstance(){ return s_instance; }

	readonly int MaxLives = 3;
	int m_lives;

	public delegate void OnLifeChanged(int lives);
	public OnLifeChanged onLifeChanged;


	[SerializeField]
	AudioSource m_source;

	void Awake () 
	{
		if(!s_instance) s_instance = this;
	}

	public void ResetLives()
	{
		m_lives = MaxLives;

		if (onLifeChanged != null)
			onLifeChanged(m_lives);
	}

	public void AddLife()
	{
		m_lives++;
		if (m_lives >= MaxLives)
			m_lives = MaxLives;

		if (onLifeChanged != null)
			onLifeChanged(m_lives);
	}

	public void RemoveLife()
	{
		m_source.Play ();

		m_lives--;

		if (m_lives <= 0)
			m_lives = 0;
			
		if (onLifeChanged != null)
			onLifeChanged(m_lives);	
	}

}
