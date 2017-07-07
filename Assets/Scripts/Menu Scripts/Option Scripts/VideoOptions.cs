using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamUtility.IO;
using UnityEngine.UI;
using System.Linq;
using System;

namespace SeaOfGreed{
	public class VideoOptions : MonoBehaviour {
		public Canvas parent;
		private Canvas canvas;
		private Resolution[] resolutions;
		public Dropdown resolutionDropdown;
		public Toggle fullscreenToggle;
		public Button applyButton;

		private bool fullscreen;
		private Resolution resolution;

		public delegate void OnResolutionChanged(object sender, EventArgs e);
		public event OnResolutionChanged ResolutionChanged;

		void Start(){
			canvas = GetComponent<Canvas> ();
			resolutions = Screen.resolutions;
			resolutionDropdown.ClearOptions();
			resolutionDropdown.AddOptions (resolutions.Select ((x) => {
				//Fkin C# 4.0 cant do this:  return $"{x.width} X {x.height} {x.refreshRate}";
				return x.width + " X " + x.height + " " + x.refreshRate;
			}).ToList());

		}

		void Update(){
			if (ChangeMade ()) {
				applyButton.interactable = true;
			} else {
				applyButton.interactable = false;
			}
		}

		public void Load(){
			fullscreenToggle.isOn = GameManager.options.video.fullscreen;
			resolutionDropdown.value = resolutions.ToList ().FindIndex((x) => {return x.width == GameManager.options.video.width && x.height == GameManager.options.video.height;});
		}

		public void OnFullscreenToggle(bool value){
			 fullscreen = value;
		}

		public void OnResolutionDropdownChange(int index){
			resolution = resolutions [index];
		}

		public void OnApply(){
			GameManager.options.video.fullscreen = fullscreen;
			GameManager.options.video.width = resolution.width;
			GameManager.options.video.height = resolution.height;
			Screen.SetResolution (resolution.width, resolution.height, fullscreen);
		}

		bool ChangeMade(){
			if (fullscreenToggle.isOn != GameManager.options.video.fullscreen) {
				return true;
			}
			return false;
		}
	}
}
