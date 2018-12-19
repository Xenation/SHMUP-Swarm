using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossLife : MonoBehaviour
{
	[SerializeField]
	private int pv = 1;
	public bool isPart = true;

    private bool inHitStun = false;
    private Color originalColor;
    private float hitStunDuration = 0.05f;
    private float hitStunFirstFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = this.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (inHitStun == true)
        {
            hitstun();
        }
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!isPart && col.gameObject.layer == LayerMask.NameToLayer("ProjectileUnit"))
		{
			lowerPV();
            hitstun();
		}
	}

	void lowerPV()
	{
		pv--;
		if (pv <= 0)
		{
            SceneManager.LoadScene("Win");
            //Destroy(this.gameObject);
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

    private void hitstun()
    {
        if (inHitStun == false)
        {
            inHitStun = true;
            this.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f, 1);
            hitStunFirstFrame = Time.time;
        }
        else if (Time.time > (hitStunFirstFrame + hitStunDuration))
        {
            inHitStun = false;
            this.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("died");
        //SceneManager.LoadScene("Win");
    }

}
