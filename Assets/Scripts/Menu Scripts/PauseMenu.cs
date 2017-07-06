using System;
using TeamUtility.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SeaOfGreed
{
	public class PauseMenu : MonoBehaviour
	{
		public bool paused = false;
		private Canvas canvas;
		public Canvas optionsCanvas;
		public Canvas gameCanvas;
		public Canvas videoCanvas;
		public Canvas audioCanvas;
		public Canvas controlsCanvas;

		void Start(){
			canvas = GetComponent<Canvas> ();
			canvas.enabled = false;
			optionsCanvas.enabled = false;
			gameCanvas.enabled = false;
			videoCanvas.enabled = false;
			audioCanvas.enabled = false;
			controlsCanvas.enabled = false;
		}

		void Update(){
			
			if (InputManager.GetButtonDown ("Pause")) {
				if (!paused) {
					Pause ();
				} else if (canvas.enabled || optionsCanvas.enabled) {
					Unpause ();
				} else {
					ToOptions ();
				}
			}
		}

		public void Pause(){
			canvas.enabled = true;
			Time.timeScale = 0f;
			paused = true;
		}

		public void Unpause(){
			canvas.enabled = false;
			optionsCanvas.enabled = false;
			gameCanvas.enabled = false;
			videoCanvas.enabled = false;
			audioCanvas.enabled = false;
			controlsCanvas.enabled = false;
			Time.timeScale = 1f;
			paused = false;
		}

		public void Quit(){
			GameManager.gameManager.Save ();
			SceneManager.LoadScene ("Menu");
		}

		public void ReturnToMainMenu(){
			canvas.enabled = true;
			optionsCanvas.enabled = false;
		}

		public void GameSettings(){
			canvas.enabled = false;
			optionsCanvas.enabled = false;
			gameCanvas.gameObject.GetComponent<GameOptions> ().Load ();
			gameCanvas.enabled = true;
		}

		public void VideoSettings(){
			canvas.enabled = false;
			optionsCanvas.enabled = false;
			videoCanvas.gameObject.GetComponent<VideoOptions> ().Load ();
			videoCanvas.enabled = true;
		}

		public void AudioSettings(){
			canvas.enabled = false;
			optionsCanvas.enabled = false;
			audioCanvas.gameObject.GetComponent<AudioOptions> ().Load ();
			audioCanvas.enabled = true;
		}

		public void ControlsSettings(){
			canvas.enabled = false;
			optionsCanvas.enabled = false;
			controlsCanvas.gameObject.GetComponent<ControlOptions> ().Load ();
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
}

