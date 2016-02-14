using UnityEngine;
using System.Collections;

public delegate void ChangeBossHPHandler(int hp);

public class EnemyBossScript : MonoBehaviour {

    public int hp;
    public float moveTime = 1.5f;
    public float rpmSpin;
    public Transform hivePos;
    public Transform exposePos;
    public GameObject player;
    public GameObject BossHealthBar;
    public GameObject redExplosion;

    public static event ChangeBossHPHandler onBossHPChanged;
    public static event ChangeBossHPHandler onBossMaxHPChanged;

    private Rigidbody2D rb2D;
    private Vector3 end;
    private float angle;
    private float inverseMoveTime;
    private float fireRange;
    private Vector3 playPos;
    private int baseHP = 20;
    private int newHP;

    // Use this for initialization
    void Start () {
        rb2D = GetComponent<Rigidbody2D>();
        angle = rpmSpin * 6f;
        inverseMoveTime = 1f / moveTime;        
        fireRange = player.GetComponent<AllyShipScript>().fireRange;
        playPos = player.transform.position;
    }

    // Update is called once per frame
    void Update () {
        StartCoroutine(SmoothMovement());
        transform.Rotate(Vector3.forward, angle * Time.deltaTime);
        if (end == hivePos.position && Vector2.SqrMagnitude(transform.position - playPos) > fireRange)
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    IEnumerator SmoothMovement()
    {
        float sqrMagnitudeRemaining = (end - transform.position).sqrMagnitude;
        while (sqrMagnitudeRemaining > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            yield return null;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "DisableTrigger")
        {
            gameObject.SetActive(false);
            GameManager.instance.Invoke("StartNewLevel", 1f);
        }
    }

    public void Attack()
    {
        BossHealthBar.SetActive(true);
        end = exposePos.position;
        newHP = Mathf.RoundToInt(baseHP * (1f + (GameManager.instance.level -1) / 10f));
        if (onBossMaxHPChanged != null)
        {
            onBossMaxHPChanged(newHP);
        }
        ModifyBossHP(newHP);
    }

    public void Retreat()
    {
        BossHealthBar.SetActive(false);
        end = hivePos.position;
    }

    public void CheckWin()
    {
        if (hp == 0)
        {
            //SoundManager.instance.PlayExplosion();
            GetComponent<Collider2D>().enabled = false;
            player.SendMessage("ModifyAmmunition", newHP * 1.5f);
            Retreat();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet")
        {
            //SoundManager.instance.PlayExplosion();
            ModifyBossHP(-1);
            CheckWin();
            Instantiate(redExplosion, other.transform.position, Quaternion.identity);
        }
    }

    void ModifyBossHP(int amount)
    {
        hp += amount;
        if (onBossHPChanged != null)
        {
            onBossHPChanged(hp);
        }
    }
}
