using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public float bulletSpeed;
    public Transform target { set; get; }

    private Rigidbody2D rb2;
    private Vector3 newPos;
    private Vector3 playerPos;
    private Vector3 targetPos;
    private Vector3 dir;

    // Use this for initialization
    void Start() {
        rb2 = GetComponent<Rigidbody2D>();
        newPos = transform.position;
        if (tag == "PlayerBullet")
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    // Update is called once per frame
    void Update() {
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        while (true)
        {
            if (target == null)
            {
                dir = newPos - playerPos;
                newPos += dir.normalized * bulletSpeed * Time.deltaTime;
                rb2.MovePosition(newPos);
                yield return null;
            }
            else
            {
                targetPos = target.position;
                dir = targetPos - newPos;
                newPos += dir.normalized * bulletSpeed * Time.deltaTime;
                rb2.MovePosition(newPos);
                dir = targetPos - newPos;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                yield return null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Shield" && other.tag != "PlayerBullet")
        {
            Destroy(gameObject);
        }
    }
}