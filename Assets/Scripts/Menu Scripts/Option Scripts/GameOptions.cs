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
			MinimapWithPlayer.isOn = MinimapCameraFollow.RotateWithPlayer;
			MinimapWithShip.isOn = MinimapCameraFollow.RotateWithShip;
		}

		public void OnMinimapWithPlayerChanged(bool value){
			//Debug.Log ("ChangedPlayer" + value.ToString ());
			MinimapCameraFollow.RotateWithPlayer = value;
		}

		public void OnMinimapWithShipChanged(bool value){
			//Debug.Log ("ChangedShip" + value.ToString ());
			MinimapCameraFollow.RotateWithShip = value;
		}
	}
}