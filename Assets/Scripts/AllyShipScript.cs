using UnityEngine;
using System.Linq;
using System.Collections;

public delegate void ChangeBulletQtyHandler(int qty);

public class AllyShipScript : MonoBehaviour {

    public int hp = 3;
    public float rotateSpeed = 5f;    
    public float fireRange = 40;
    public GameObject bullet;
    public Transform muzzle;
    public int ammunition;
    public GameObject blueExplosion;

    public static event ChangeBulletQtyHandler onBulletQtyChanged;

    private Vector3 allyPos;
    private AudioSource fireSound;

    // Use this for initialization
    void Start () {
        allyPos = new Vector2(transform.position.x - 0.05f, transform.position.y);
        fireSound = GetComponent<AudioSource>();
        ModifyAmmunition(40);
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(Rotating());
        StartCoroutine(Shooting());
	}

    GameObject ClosestEnemy()
    {
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float distance = fireRange;
        foreach (GameObject enemy in enemyList)
        {
            float curDistance = (enemy.transform.position - allyPos).sqrMagnitude;
            if (curDistance < distance)
            {
                closestEnemy = enemy;
                distance = curDistance;
            }
        }
        return closestEnemy;
    }

    IEnumerator Rotating()
    {
        GameObject target = ClosestEnemy();
        if (target == null)
        {
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotateSpeed);
            yield break;
        }

        Vector3 dir = target.transform.position - allyPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion newRot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, newRot, Time.deltaTime * rotateSpeed);
        yield return null;
    }

    IEnumerator Shooting()
    {
        //Cast a ray forward to find enemy
        RaycastHit2D hit = Physics2D.Raycast(allyPos, muzzle.position - allyPos);
        //Detected object
        if (hit.collider != null)
        {
            Collider2D cld = hit.collider;
            if (cld.tag == "Enemy" && Vector2.SqrMagnitude(cld.transform.position - allyPos) <= fireRange)
            {
                //Check if object is targeted by an existing bullet OR out of ammunition
                if (IsTargeted(cld.transform) || ammunition <= 0)
                {
                    yield break;
                }
                //Fire a bullet
                GameObject b = Instantiate(bullet, muzzle.position, Quaternion.identity) as GameObject;
                fireSound.Play();
                b.GetComponent<BulletScript>().target = cld.transform;
                ModifyAmmunition(-1);
                yield return null;
            }
        }
        else
        {
            yield break;
        }
    }

    bool IsTargeted(Transform t)
    {
        GameObject[] activeBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        foreach (GameObject bullet in activeBullets)
        {
            Transform target = bullet.GetComponent<BulletScript>().target;
            if (target == t)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckGameOver()
    {
        if (hp <= 0)
        {
            //SoundManager.instance.PlayExplosion();
            Destroy(gameObject);
            Instantiate(blueExplosion, allyPos, Quaternion.identity);
            GameManager.instance.Invoke("GameOver", 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.tag == "Enemy")
        //{
        //    gameObject.SetActive(false);
        //    Destroy(other.gameObject);
        //    Destroy(gameObject);
        //    GameManager.instance.Invoke("GameOver", 0f);
        //}
        //else 
        if (other.tag == "EnemyBullet")
        {
            Debug.Log("Player get hit");
            //SoundManager.instance.PlayExplosion();
            hp -= 1;
            CheckGameOver();
            Instantiate(blueExplosion, allyPos, Quaternion.identity);
        }
    }

    public void ModifyAmmunition(int qty)
    {
        ammunition += qty;
        if (onBulletQtyChanged != null)
        {
            onBulletQtyChanged(ammunition);
        }
    }
}
