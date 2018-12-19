using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossLife : MonoBehaviour
{
	[SerializeField]
	private int pv = 1;
	public bool isPart = true;
	private Animator animator;
	public float openingDuration= 5;
	private float openingTime = 2;
	private bool isAnimationEnd= false;

    private bool inHitStun = false;
    private Color originalColor;
    private float hitStunDuration = 0.05f;
    private float hitStunFirstFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
		animator = gameObject.GetComponent<Animator>();
        originalColor = this.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
		if (!isPart && isAnimationEnd && Time.time - openingTime > openingDuration)
		{
			animator.SetBool("isOpen", false);
			isPart = true;
            isAnimationEnd = false;

            foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
			{
				part.GetComponent<partController>().resetPart();
			}
		}
        if (inHitStun == true)
        {
            hitstun();
        }
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!isPart && isAnimationEnd && col.gameObject.layer == LayerMask.NameToLayer("ProjectileUnit"))
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
				animator.SetBool("isOpen",true);
				//part.SetActive(false);
			}
		}
	}

    private void hitstun()
    {
        if (!inHitStun)
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
        else
        {
            //ScreenShake

        }
    }

    private void OnDestroy()
    {
        //Debug.Log("died");
        //SceneManager.LoadScene("Win");
    }

	public void AlertObservers(string message)
	{
		if (message.Equals("AttackAnimationEnded"))
		{
			isAnimationEnd = true;
			openingTime = Time.time;
		}
	}
}
