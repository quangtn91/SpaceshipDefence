using UnityEngine;
using System.Collections;

public class ExplosionSound : MonoBehaviour {

    public AudioClip[] explosion;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    private AudioSource efxSource;

    // Use this for initialization
    void Start () {
        transform.Rotate(Vector3.forward, Random.Range(0, 360)); 
        efxSource = gameObject.AddComponent<AudioSource>();
        PlayExplosion();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayExplosion()
    {
        efxSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.clip = explosion[Random.Range(0, explosion.Length)];
        efxSource.Play();
    }
}
