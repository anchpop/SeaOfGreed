using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeamUtility.IO;

public class MainMenu : MonoBehaviour {
	private Canvas canvas;
	public Canvas optionsCanvas;
	public Canvas gameCanvas;
	public Canvas videoCanvas;
	public Canvas audioCanvas;
	public Canvas controlsCanvas;

	void Start(){
		canvas = GetComponent<Canvas>();
		optionsCanvas.enabled = false;
		gameCanvas.enabled = false;
		videoCanvas.enabled = false;
		audioCanvas.enabled = false;
		controlsCanvas.enabled = false;
	}

	void Update (){
		if (optionsCanvas.enabled) {
			if (InputManager.GetButtonDown ("Pause")) {
				ReturnToMainMenu ();
			}
		} else if(!optionsCanvas.enabled && !canvas.enabled){
			if (InputManager.GetButtonDown ("Pause")) {
				ToOptions ();
			}
		}
	}

	public void Play(){
		SceneManager.LoadScene("testscene1");
	}

	public void ReturnToMainMenu(){
		canvas.enabled = true;
		optionsCanvas.enabled = false;
	}
		
	public void GameSettings(){
		canvas.enabled = false;
		optionsCanvas.enabled = false;
		gameCanvas.enabled = true;
	}

	public void VideoSettings(){
		canvas.enabled = false;
		optionsCanvas.enabled = false;
		videoCanvas.enabled = true;
	}

	public void AudioSettings(){
		canvas.enabled = false;
		optionsCanvas.enabled = false;
		audioCanvas.enabled = true;
	}

	public void ControlsSettings(){
		canvas.enabled = false;
		optionsCanvas.enabled = false;
		controlsCanvas.enabled = true;
	}

	public void ToOptions(){
		optionsCanvas.enabled = true;
		gameCanvas.enabled = false;
		videoCanvas.enabled = false;
		audioCanvas.enabled = false;
		controlsCanvas.enabled = false;
		canvas.enabled = false;
	}
}
