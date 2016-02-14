using UnityEngine;
using System.Collections;

public class SpawnPath : MonoBehaviour {

    public float curve;
    public Transform player;

    Vector3 a, b;

	// Use this for initialization
	void Start () {
        a = transform.position;
        b = player.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(a, b);
        float count = 20;
        Vector3 lastP = a;
        for (float i = 0; i < count + 1; i++)
        {
            Vector3 p = SampleParabola(a, b, curve, i / count);
            Gizmos.DrawLine(lastP, p);
            lastP = p;
        }
    }

    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x + 1, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            //if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }
}
