using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnStartClick () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	} 

	public void OnOptionsClick () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (3);
	} 

	public void OnCreditsClick () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (2);
	} 

	public void OnQuitClick () {
		Application.Quit() ;
	
	}
}
