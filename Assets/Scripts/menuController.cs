using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
	private GameObject buttonPlay;

	private GameObject option;

	private GameObject menu;

	private GameObject credit;

	// Start is called before the first frame update
	void Start()
    {
		GameObject.Find("ButtonPlay").GetComponent<Button>().onClick.AddListener(onClickPlay);
		GameObject.Find("ButtonTutorial").GetComponent<Button>().onClick.AddListener(onClickTutorial);
		GameObject.Find("ButtonOption").GetComponent<Button>().onClick.AddListener(openOptions);
		GameObject.Find("ButtonExit").GetComponent<Button>().onClick.AddListener(onClickExit);
		GameObject.Find("ButtonCredit").GetComponent<Button>().onClick.AddListener(openCredit);
		GameObject.Find("ButtonRetourCredit").GetComponent<Button>().onClick.AddListener(returnMenuCredit);
		GameObject.Find("ButtonRetourOption").GetComponent<Button>().onClick.AddListener(returnMenuOption);
		option = GameObject.Find("Option");
		menu = GameObject.Find("Menu");
		credit = GameObject.Find("Credit");

	
		option.SetActive(false);
		credit.SetActive(false);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void onClickPlay()
	{
		SceneManager.LoadScene("Level1");
	}

	void onClickExit()
	{
		Application.Quit();
	}

	void onClickTutorial()
	{
		SceneManager.LoadScene("Tutorial");
	}

	void openOptions()
	{
		menu.SetActive(false);
		GameObject myEventSystem = GameObject.Find("EventSystem");
		option.SetActive(true);
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("MusicSlider"));
	}

	void openCredit()
	{
		menu.SetActive(false);
		GameObject myEventSystem = GameObject.Find("EventSystem");
		credit.SetActive(true);
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ButtonRetourCredit"));
	}

	void returnMenuOption()
	{
		menu.SetActive(true);
		GameObject myEventSystem = GameObject.Find("EventSystem");
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ButtonPlay"));
		option.SetActive(false);
	}

	void returnMenuCredit()
	{
		menu.SetActive(true);
		GameObject myEventSystem = GameObject.Find("EventSystem");
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(GameObject.Find("ButtonPlay"));
		credit.SetActive(false);
	}
}
