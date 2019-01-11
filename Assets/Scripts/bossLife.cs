using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossLife : MonoBehaviour
{
	[SerializeField]
	private int pv;
    private int phase2Threshhold;
    private int phase3Threshhold;
	public bool isPart = true;
	private Animator animator;
    public Camera cam;

	public float openingDuration= 5;
	private float openingTime = 2;
	private bool isAnimationEnd= false;

    private bool inHitStun = false;
    private Material mat;

    public float hitStunDuration = 0.05f;
    private float hitStunFirstFrame = 0;

    private float ScoreTimer = 0;

    //Ajouter une référence vers chaque part du boss


    private void Awake()
    {
        ScoreTimer = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {
		animator = gameObject.GetComponent<Animator>();
        SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
        mat = rend.material;
    }

    // Update is called once per frame
    void Update()
    {
		if (!isPart && isAnimationEnd && Time.time - openingTime > openingDuration)
		{
			animator.SetBool("isOpen", false);
            AkSoundEngine.PostEvent("Play_BossClose", gameObject);
            isPart = true;
            isAnimationEnd = false;

            foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
			{
				part.GetComponent<partController>().resetPart();
			}

            CheckPhase();
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
            
            Invoke("End", 1.0f);

            //Destroy(this.gameObject);
        }
        else
        {
            //INSERER SON DEGAT SUR BOSS
            AkSoundEngine.PostEvent("Play_CoeurHit", gameObject);
        }
	}

    public void End()
    {
        ScoreTimer = Time.time - ScoreTimer;
        //Envoyez le score dans la prochaine scene + leaderboard
        

        SceneManager.LoadScene("Win");
        
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
			openingTime = Time.time;
			foreach (GameObject part in GameObject.FindGameObjectsWithTag("part"))
			{
				animator.SetBool("isOpen",true);
                //part.SetActive(false);
                AkSoundEngine.PostEvent("Play_BossOpen", gameObject);
			}
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
        else if (Time.time > (hitStunFirstFrame + hitStunDuration))
        {
            inHitStun = false;
            mat.SetFloat("_ReplaceAmount", 0.0f);
            //cam.transform.position =new Vector3(0, 0, -10);
        }
        else
        {
            //ScreenShake
            cam.transform.position += new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
            

        }
    }

    private void CheckPhase()
    {
        if(pv < phase3Threshhold)
        {
            //Change sprite and patterns to second phase
        }else if(pv < phase2Threshhold)
        {
            //Change sprite and patterns to third phase
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
		}
	}
}
