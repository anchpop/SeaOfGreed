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

		public Button ShipEnter;
		public Button ShipAccelerate;
		public Button ShipBrake;
		public Button ShipLeft;
		public Button ShipRight;

		public Button Use;
		public Button Pause;



		void Start(){
			canvas = GetComponent<Canvas> ();
			SetUpHandlers ();
			Load ();

		}

		void SetUpHandlers(){
			PlayerForward.onClick.AddListener (OnPlayerForward);
			PlayerBackward.onClick.AddListener(OnPlayerBackward);
			PlayerLeft.onClick.AddListener(OnPlayerLeft);
			PlayerRight.onClick.AddListener(OnPlayerRight);


			ShipEnter.onClick.AddListener (OnShipEnter);
			ShipAccelerate.onClick.AddListener(OnShipAccelerate);
			ShipBrake.onClick.AddListener(OnShipBrake);
			ShipLeft.onClick.AddListener(OnShipLeft);
			ShipRight.onClick.AddListener (OnShipRight);

			Use.onClick.AddListener (OnUse);
			Pause.onClick.AddListener (OnPause);

		}

		public void Load(){
			PlayerForward.interactable = true;
			PlayerBackward.interactable = true;
			PlayerLeft.interactable = true;
			PlayerRight.interactable = true;

			ShipEnter.interactable = true;
			ShipAccelerate.interactable = true;
			ShipBrake.interactable = true;
			ShipLeft.interactable = true;
			ShipRight.interactable = true;

			Use.interactable = true;
			Pause.interactable = true;

			PlayerForward.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Forward").positive.ToString ();
			PlayerBackward.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Backward").positive.ToString ();
			PlayerRight.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Right").positive.ToString ();
			PlayerLeft.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Left").positive.ToString ();

			ShipEnter.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Enter Ship").positive.ToString ();
			ShipAccelerate.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Accelerate").positive.ToString ();
			ShipBrake.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Brake").positive.ToString ();
			ShipLeft.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Left").positive.ToString ();
			ShipRight.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Right").positive.ToString ();

			Use.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Use").positive.ToString ();
			Pause.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Pause").positive.ToString ();
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




		public void OnShipEnter(){
			ShipEnter.interactable = false;
			ShipEnter.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Enter Ship";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);

		}

		public void OnShipAccelerate(){
			ShipAccelerate.interactable = false;
			ShipAccelerate.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Accelerate";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}

		public void OnShipBrake(){
			ShipBrake.interactable = false;
			ShipBrake.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Brake";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}

		public void OnShipLeft(){
			ShipLeft.interactable = false;
			ShipLeft.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Left";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}

		public void OnShipRight(){
			ShipRight.interactable = false;
			ShipRight.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Right";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}



		public void OnUse(){
			Use.interactable = false;
			Use.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Use";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}

		public void OnPause(){
			Pause.interactable = false;
			Pause.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Pause";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
		}
	}
}
