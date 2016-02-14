using UnityEngine;
using System.Collections;

public class CheckHit : MonoBehaviour {

    public GameObject blueExplosion;

    //OnTriggerEnter2D not working with rotating object approach
    void OnTriggerStay2D(Collider2D other)
    {
        //SoundManager.instance.PlayExplosion();
        if (other.tag == "Enemy")
        {
            gameObject.SetActive(false);
            Destroy(other.gameObject);
            Destroy(gameObject);
            Instantiate(blueExplosion, transform.position, Quaternion.identity);
            GameManager.instance.Invoke("GameOver", 0f);
        }
    }
}
