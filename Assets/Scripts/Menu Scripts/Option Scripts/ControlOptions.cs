using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamUtility.IO;
using UnityEngine.UI;

namespace SeaOfGreed{
	public class ControlOptions : MonoBehaviour {
		public Canvas parent;
		private Canvas canvas;

		public Button PlayerForward;
		public Button PlayerBackward;
		public Button PlayerLeft;
		public Button PlayerRight;



		void Start(){
			canvas = GetComponent<Canvas> ();
			Load ();

		}

		void SetUpHandlers(){
			PlayerForward.onClick.AddListener (OnPlayerForward);
			PlayerBackward.onClick.AddListener(OnPlayerBackward);
			PlayerLeft.onClick.AddListener(OnPlayerLeft);
			PlayerRight.onClick.AddListener(OnPlayerRight);


		}

		public void Load(){
			PlayerForward.interactable = true;
			PlayerBackward.interactable = true;
			PlayerLeft.interactable = true;
			PlayerRight.interactable = true;

			PlayerForward.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Forward").positive.ToString ();
			PlayerBackward.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Backward").positive.ToString ();
			PlayerRight.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Right").positive.ToString ();
			PlayerLeft.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Left").positive.ToString ();
		}

		bool HandleKeyScan(ScanResult result){
			
			var axis = InputManager.GetAxisConfiguration ("Default" , result.userData.ToString());
			axis.positive = result.key;
			Load ();
			return true;
		}

		public void OnPlayerForward(){
			PlayerForward.interactable = false;
			PlayerForward.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Forward";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);

		}

		public void OnPlayerBackward(){
			PlayerBackward.interactable = false;
			PlayerBackward.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Backward";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}

		public void OnPlayerLeft(){
			PlayerLeft.interactable = false;
			PlayerLeft.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Left";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}

		public void OnPlayerRight(){
			PlayerRight.interactable = false;
			PlayerRight.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Right";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}
	}
}
