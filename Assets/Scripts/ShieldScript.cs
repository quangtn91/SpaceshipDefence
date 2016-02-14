using UnityEngine;
using System.Collections;

public class ShieldScript : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        SoundManager.instance.PlayExplosion();
    //        Destroy(other.gameObject);
    //        GameManager.instance.takeDown += 1;
    //    }
    //}

    void OnEnable()
    {
        player.GetComponent<Collider2D>().enabled = false;
    }

    void OnDisable()
    {
        player.GetComponent<Collider2D>().enabled = true;
    }

}
