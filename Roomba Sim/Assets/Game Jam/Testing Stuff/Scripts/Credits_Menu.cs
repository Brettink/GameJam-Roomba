using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits_Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoBack () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
        Object.Destroy(MenuMus.self.gameObject);

    }
}
