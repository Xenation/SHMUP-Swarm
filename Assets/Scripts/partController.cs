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
    // Start is called before the first frame update
    void Start()
    {
		pv = basepv;
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
			this.GetComponent<SpriteRenderer>().color = Color.red;
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
            this.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 0.4f, 1);
            hitStunFirstFrame = Time.time;
        }
        else if(Time.time > (hitStunFirstFrame + hitStunDuration))
        {
            inHitStun = false;
            this.GetComponent<SpriteRenderer>().color = Color.white;
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
