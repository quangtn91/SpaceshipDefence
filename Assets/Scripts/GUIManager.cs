using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

    public Text BulletText;
    public Text BossHealthText;
    public Slider BossHealthBar;

    // Use this for initialization
    void Start () {
        AllyShipScript.onBulletQtyChanged += AllyShipScript_onBulletQtyChanged;
        EnemyBossScript.onBossHPChanged += EnemyBossScript_onBossHPChanged;
        EnemyBossScript.onBossMaxHPChanged += EnemyBossScript_onBossMaxHPChanged;
	}

    private void EnemyBossScript_onBossMaxHPChanged(int hp)
    {
        BossHealthBar.maxValue = hp;
    }

    private void EnemyBossScript_onBossHPChanged(int hp)
    {
        BossHealthText.text = hp.ToString();
        BossHealthBar.value = hp;
    }

    private void AllyShipScript_onBulletQtyChanged(int qty)
    {
        BulletText.text = qty.ToString();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
