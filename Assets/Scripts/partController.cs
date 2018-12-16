using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partController : MonoBehaviour
{

	[SerializeField]
	public int pv = 1;
	public bool isDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
