﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MainMenu() {
        SceneManager.LoadScene("mainMenu");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
