using UnityEngine;
using System.Collections;

public class SelfDestruction : MonoBehaviour {

    public float aliveTime = 1.417f;
	
	// Update is called once per frame
	void Update () {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }
	}
}
