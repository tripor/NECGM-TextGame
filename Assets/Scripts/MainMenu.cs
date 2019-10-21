using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void clickPlay()
	{
		Debug.Log("Changing Scene");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void clickExit()
	{
		Debug.Log("Exiting");
		Application.Quit();
		
	}
}
