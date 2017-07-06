using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	private Canvas canvas;
	public Canvas optionsCanvas;

	void Start(){
		canvas = GetComponent<Canvas>();
		optionsCanvas.enabled = false;
	}

	public void Play(){
		SceneManager.LoadScene("testscene1");
	}

	public void Options(){
		canvas.enabled = false;
		optionsCanvas.enabled = true;
	}

	public void ReturnToMainMenu(){
		canvas.enabled = true;
		optionsCanvas.enabled = false;
	}
}
