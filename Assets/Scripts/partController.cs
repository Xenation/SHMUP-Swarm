using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partController : MonoBehaviour
{

	[SerializeField]
	private int pv = 1;
	public int basepv = 1;
	public bool isDestroyed = false;

    private bool inHitStun = false;
    private float hitStunFirstFrame = 0;
    private float hitStunDuration = 0.05f;
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
        }
        else
        {
            hitstun();
        }
	}

    private void hitstun()
    {
        if (inHitStun == false)
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

	public void resetPart()
	{
		isDestroyed = false;
		pv = basepv;
		this.GetComponent<SpriteRenderer>().color = Color.white;
	}

	public void animationEnd(bool endAnimation)
	{
	
	}
}
