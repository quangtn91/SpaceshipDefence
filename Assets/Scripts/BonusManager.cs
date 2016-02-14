using UnityEngine;
using System.Collections;

public class BonusManager : MonoBehaviour {

    private int energy;

	// Use this for initialization
	void Start () {
        EnemyShipScript.onEnemyShipDestroy += EnemyShipScript_onEnemyShipDestroy;
	}

    private void EnemyShipScript_onEnemyShipDestroy()
    {
        energy += 1;        
    }

    // Update is called once per frame
    void Update () {
	
	}
}
