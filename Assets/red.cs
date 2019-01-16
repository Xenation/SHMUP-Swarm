using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class red : MonoBehaviour
{
    private Image redIm;
    // Start is called before the first frame update
    void Start()
    {
        redIm = GetComponent<Image>();
        redIm.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (redIm.enabled == false)
        {
            int r = Random.Range(0, 6000);
            if(r == 0)
            {
                redIm.transform.localPosition = new Vector3(Random.Range(-400f, 400f), Random.Range(-400f, 400f), redIm.transform.position.z);
                redIm.transform.localScale *= Random.Range(0.5f, 1.5f);
                redIm.enabled = true;
            }
        }
        else
        {
            redIm.enabled = false;
        }
    }
}
