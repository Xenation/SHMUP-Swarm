using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BRPyu : MonoBehaviour
{
    public string team;
    public Tournament tournament;
    public int hp;
    public int dmgPerSecond;
    public int speed;
    public Color color;

    private BRPyu closestPyu;
    private Vector3 celerity;
    private float firstHitTime;

    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        float minDist = 0;
        if(tournament.contenders.Count > 1 && tournament.nbOfTeamsLeft() > 1)
        {
            foreach (BRPyu pyu in tournament.contenders)
            {
                if (team != pyu.team)
                {
                    float dist = Mathf.Sqrt(Mathf.Pow(pyu.transform.position.x - transform.position.x, 2) +
                        Mathf.Pow(pyu.transform.position.y - transform.position.y, 2));

                    if (minDist == 0 || dist < minDist)
                    {
                        minDist = dist;
                        closestPyu = pyu;
                    }
                }
            }

            Vector3 dir = new Vector3(closestPyu.transform.position.x - transform.position.x, closestPyu.transform.position.y - transform.position.y, 0);
            dir.Normalize();
            dir *= speed;
            celerity = dir;
        }
        else
        {
            celerity = Vector3.zero;
        }

        if (color != sr.color)
            sr.color = color;

    }

    private void FixedUpdate()
    {
        rb.velocity = celerity;
        rb.rotation = Vector2.SignedAngle(Vector2.up, celerity.normalized);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        BRPyu pyu = collision.gameObject.GetComponent<BRPyu>();
        if (pyu && pyu.team != team)
        {
            lowerHP();
            firstHitTime = Time.time;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Add damage per second
        BRPyu pyu = collision.gameObject.GetComponent<BRPyu>();
        if (pyu && pyu.team != team && Time.time > (firstHitTime + (1.0f / (float)dmgPerSecond)))
        {
            lowerHP();
            firstHitTime = Time.time;
        }
    }

    private void lowerHP()
    {
        Debug.Log("here");
        if(hp > 0)
        {
            hp--;
        }
        else
        {
            tournament.contenders.Remove(this);
            Destroy(gameObject);
        }
    }

}
