using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partController : MonoBehaviour
{

	[SerializeField]
	private int pv = 1;
	public int basepv = 1;
	public bool isDestroyed = false;
    public Camera cam;

    private bool inHitStun = false;
    private bool inDestroyShake = false;
    private float hitStunFirstFrame = 0;
    private float hitStunDuration = 0.05f;
    private float destroyDuration = 0.2f;
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
		pv = basepv;
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        mat = rend.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (inHitStun)
            hitstun();
        if (inDestroyShake)
            destroyShake();
    }

    private void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.layer == 11 && !isDestroyed)
		{
			lowerPV();
		}
	}

	void lowerPV()
	{
		pv--;
		if (pv <= 0)
		{
			isDestroyed = true;
            mat.SetFloat("_ReplaceAmount", 0.5f);
			transform.parent.GetComponent<bossLife>().checkParts();
            destroyShake();
        }
        else
        {
            hitstun();
        }
	}

    private void hitstun()
    {
        if (!inHitStun)
        {
            inHitStun = true;
            mat.SetFloat("_ReplaceAmount", 1.0f);
            hitStunFirstFrame = Time.time;
        }
        else if(Time.time > (hitStunFirstFrame + hitStunDuration))
        {
            inHitStun = false;
            mat.SetFloat("_ReplaceAmount", 0.0f);
        }
    }

    private void destroyShake()
    {
        if (!inDestroyShake)
        {
            inDestroyShake = true;
            hitStunFirstFrame = Time.time;
        }
        else if(Time.time > (hitStunFirstFrame + destroyDuration))
        {
            inDestroyShake = false;
        }
        else
            cam.transform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
    }

	public void resetPart()
	{
		isDestroyed = false;
		pv = basepv;
        mat.SetFloat("_ReplaceAmount", 0.0f);
	}

	public void animationEnd(bool endAnimation)
	{
	
	}
}
