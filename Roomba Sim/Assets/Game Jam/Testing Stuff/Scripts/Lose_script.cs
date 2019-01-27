using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayAgain () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (4);

	}

	public void MainMenu () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);

	}
}
