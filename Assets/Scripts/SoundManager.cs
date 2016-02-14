using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioClip[] explosion;    

    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            efxSource = gameObject.AddComponent<AudioSource>();
            efxSource.playOnAwake = false;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }

    public void PlayExplosion()
    {       
        efxSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.clip = explosion[Random.Range(0, explosion.Length)];
        efxSource.Play();
    }
}
