using UnityEngine;
using System.Collections;


public delegate void TakeDownHandler();

public class EnemyShipScript : MonoBehaviour {

    public float totalMoveTime = 10f;
    public float[] curve;
    public GameObject redExplosion;

    public static event TakeDownHandler onEnemyShipDestroy;

    private float inverseMoveTime;
    private float percentMoveTime;
    private Rigidbody2D rb2D;
    private Vector3 a, b;
    private float height;

    // Use this for initialization
    void Start () {
        a = transform.position;
        b = GameObject.FindGameObjectWithTag("Player").transform.position;
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / totalMoveTime / 100;
        height = curve[Random.Range(0, curve.Length)];        
    }

    // Update is called once per frame
    void Update () {
        StartCoroutine(SmoothMovement());
    }

    IEnumerator SmoothMovement()
    {
        while (percentMoveTime <= 1f)
        {
            Vector3 p = SampleParabola(a, b, height, percentMoveTime);
            percentMoveTime += Time.deltaTime * inverseMoveTime;
            rb2D.MovePosition(p);
            Vector3 dir = b - p;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            yield return null;
        }
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        //if (Mathf.Abs(start.y - end.y) < 0.1f)
        //{            
        //    //start and end are roughly level, pretend they are - simpler solution with less steps
        //    Vector3 travelDirection = end - start;
        //    Vector3 result = start + t * travelDirection;
        //    result.y += (-parabolicT * parabolicT + 1) * height;
        //    return result;
        //}
        //else {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirection = end - new Vector3(start.x + 1, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirection);
            Vector3 up = Vector3.Cross(right, travelDirection);
            //if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        //}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBullet" || other.tag == "Shield")
        {
            //SoundManager.instance.PlayExplosion();           
            Destroy(gameObject);
            //GameManager.instance.takeDown += 1;
            if (onEnemyShipDestroy != null)
            {
                onEnemyShipDestroy();
            }
        }
    }

    void OnDestroy()
    {
        Instantiate(redExplosion, transform.position, Quaternion.identity);
    }

}
