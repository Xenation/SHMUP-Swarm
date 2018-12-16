using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossLife : MonoBehaviour
{
	[SerializeField]
	private int pv = 1;
	public bool isPart = true;

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
		if (!isPart && col.gameObject.layer == 11)
		{
			lowerPV();
		}
	}

	void lowerPV()
	{
		pv--;
		if (pv <= 0)
		{
			Destroy(this.gameObject);
		}
	}

	public void checkParts()
	{
		isPart = false;
		foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
		{
			if (!part.GetComponent<partController>().isDestroyed)
			{
				isPart = true;
			}
		}

		if (!isPart)
		{
			foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
			{
				part.SetActive(false);
			}
		}
	}

}
