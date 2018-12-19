using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partController : MonoBehaviour
{

	[SerializeField]
	private int pv = 1;
	public int basepv = 1;
	public bool isDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
		pv = basepv;
	}

    // Update is called once per frame
    void Update()
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
