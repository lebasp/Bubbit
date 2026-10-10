using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    static SoundManager s_instance;
    public static SoundManager GetInstance() { return s_instance; }

    AudioSource m_gameMusicSource;
    
    void Awake ()
    {
        if (!s_instance) s_instance = this;

        DontDestroyOnLoad(this.gameObject);

        m_gameMusicSource = GetComponent<AudioSource>();
	}

    public void SetPitch(float speedModifier)
    {
        StopCoroutine("SpeedSound");
        StartCoroutine("SpeedSound", speedModifier);
    }

    IEnumerator SpeedSound(float finalPitch)
    {
        var diff = (finalPitch - m_gameMusicSource.pitch);

        var diffRemaining = Mathf.Abs(diff);

        var iterationsNumber = 20f;

        while (diffRemaining > 0)
        {
            m_gameMusicSource.pitch += diff / iterationsNumber;

            diffRemaining -= Mathf.Abs(diff) / iterationsNumber;

            yield return null;
        }

        m_gameMusicSource.pitch = finalPitch;
    }
}
