﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
	Camera camera;
    // Start is called before the first frame update
    void Start()
    {
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		float screenHeightInUnits = Camera.main.orthographicSize ;
		float screenWidthInUnits = (screenHeightInUnits * Screen.width / Screen.height);
		transform.GetChild(0).position = new Vector3(camera.transform.position.x - screenWidthInUnits-0.5f, camera.transform.position.y, transform.GetChild(0).position.z);
		transform.GetChild(1).position = new Vector3(camera.transform.position.x + screenWidthInUnits+0.5f, camera.transform.position.y, transform.GetChild(0).position.z);
		transform.GetChild(2).position = new Vector3(camera.transform.position.x , camera.transform.position.y + screenHeightInUnits + 0.5f, transform.GetChild(0).position.z);
		transform.GetChild(3).position = new Vector3(camera.transform.position.x, camera.transform.position.y - screenHeightInUnits - 0.5f, transform.GetChild(0).position.z);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
