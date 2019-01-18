using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEye : MonoBehaviour
{

    private float startTime;

    public float timeBetweenMovement = 5.0f;
    public float dist = 5.0f;
    public float lerpRatio = 0.1f;

    private Vector3 destination;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        destination = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - startTime) > timeBetweenMovement)
        {
            float x = Random.Range(-dist, dist);
            float y = Random.Range(-dist, dist);
            Vector3 dir = new Vector3(x, y);
            dir.Normalize();
            dir *= dist;

            destination = new Vector3(65 + dir.x, 10 +  dir.y, transform.localPosition.z);
            startTime = Time.time;
        }
        else
        {
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, destination.x, 1 / lerpRatio),
                Mathf.Lerp(transform.localPosition.y, destination.y, 1/ lerpRatio),
                transform.localPosition.z);
        }
    }
}
