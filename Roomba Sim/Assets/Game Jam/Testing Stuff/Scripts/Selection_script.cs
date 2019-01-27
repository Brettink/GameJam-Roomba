using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection_script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LivingButton () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (5);

	}

	public void TheatreButton () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (6);
	
	}

}
