using System;
using UnityEngine;
using System.Linq;

namespace SeaOfGreed
{
	[Serializable]
	public class Options
	{
		public Game game;
		public Video video;
		public Audio audio;
		public Controls controls;

		public Options(){
			game = new Game ();
			video = new Video ();
			audio = new Audio ();
			controls = new Controls ();
		}

		public void Defaults(){
			game.Defaults ();
			video.Defaults ();
		}

		[Serializable]
		public class Game{
			public bool MinimapRotateWithPlayer;
			public bool MinimapRotateWithShip;
			public bool DrawUI;
			public float UIScale;


			public void Defaults(){
				MinimapRotateWithPlayer = false;
				MinimapRotateWithShip = false;
				DrawUI = true;
				UIScale = 2f;
			}
		}

		[Serializable]
		public class Video{
			public bool fullscreen;
			public int width;
			public int height;

			public void Defaults(){
				fullscreen = false;
				width = Screen.resolutions.Last().width;
				height = Screen.resolutions.Last ().height;
			}
		}

		[Serializable]
		public class Audio{

			public void Defaults(){
				
			}
		}

		[Serializable]
		public class Controls{

			public void Defaults(){

			}
		}
	}
}