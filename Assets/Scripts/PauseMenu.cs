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

		void Start(){
			canvas = GetComponent<Canvas> ();
			canvas.enabled = false;
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
		}

		public void Unpause(){
			canvas.enabled = false;
			Time.timeScale = 1f;
		}

		public void Quit(){
			SceneManager.LoadScene ("Menu");
		}
	}
}

