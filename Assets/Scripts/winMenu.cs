using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class winMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("returnToMenu").GetComponent<Button>().onClick.AddListener(onClickMenu);
        GameObject.Find("SubmitScore").GetComponent<Button>().onClick.AddListener(onClickSubmit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onClickMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void onClickSubmit()
    {
        //Mettre le score en ligne
    }
}
