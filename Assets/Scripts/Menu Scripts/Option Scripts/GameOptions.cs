using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamUtility.IO;
using System.Linq;
using UnityEngine.UI;

namespace SeaOfGreed{
	public class GameOptions : MonoBehaviour {
		public Canvas parent;
		private Canvas canvas;


		public Toggle MinimapWithPlayer;
		public Toggle MinimapWithShip;

		void Start(){
			canvas = GetComponent<Canvas> ();
		}

		public void Load(){
			MinimapWithPlayer.isOn = GameManager.options.MinimapRotateWithPlayer;
			MinimapWithShip.isOn = GameManager.options.MinimapRotateWithShip;
		}

		public void OnMinimapWithPlayerChanged(bool value){
			//Debug.Log ("ChangedPlayer" + value.ToString ());
			GameManager.options.MinimapRotateWithPlayer = value;
		}

		public void OnMinimapWithShipChanged(bool value){
			//Debug.Log ("ChangedShip" + value.ToString ());
			GameManager.options.MinimapRotateWithShip = value;
		}
	}
}