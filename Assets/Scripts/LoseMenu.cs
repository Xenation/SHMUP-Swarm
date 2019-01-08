using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("returnToMenu").GetComponent<Button>().onClick.AddListener(onClickMenu);
        GameObject.Find("Retry").GetComponent<Button>().onClick.AddListener(onClickRetry);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void onClickMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void onClickRetry()
    {
        SceneManager.LoadScene("MorganScene");
    }
}
