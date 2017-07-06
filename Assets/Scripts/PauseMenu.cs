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

		void Start(){
			canvas = GetComponent<Canvas> ();
			canvas.enabled = false;
			optionsCanvas.enabled = false;
		}

		void Update(){
			
			if (InputManager.GetButtonDown ("Pause")) {
				if (!paused) {
					Pause ();
				} else {
					Unpause ();
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
			Time.timeScale = 1f;
			paused = false;
		}

		public void Quit(){
			SceneManager.LoadScene ("Menu");
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
}

