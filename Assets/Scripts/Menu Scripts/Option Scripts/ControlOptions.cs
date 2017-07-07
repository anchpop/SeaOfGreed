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
		public Button Sprint;

		public Button ShipEnter;
		public Button ShipAccelerate;
		public Button ShipBrake;
		public Button ShipLeft;
		public Button ShipRight;

		public Button Use;
		public Button Pause;

		public bool isEditing = false;



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
			Sprint.onClick.AddListener(OnSprint);


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
			Sprint.interactable = true;

			ShipEnter.interactable = true;
			ShipAccelerate.interactable = true;
			ShipBrake.interactable = true;
			ShipLeft.interactable = true;
			ShipRight.interactable = true;

			Use.interactable = true;
			Pause.interactable = true;

			var colors = PlayerForward.colors;
			colors.colorMultiplier = 1f;

			PlayerForward.colors = colors;
			PlayerBackward.colors = colors;
			PlayerLeft.colors = colors;
			PlayerRight.colors = colors;
			Sprint.colors = colors;

			ShipEnter.colors = colors;
			ShipAccelerate.colors = colors;
			ShipBrake.colors = colors;
			ShipLeft.colors = colors;
			ShipRight.colors = colors;

			Use.interactable = true;
			Pause.interactable = true;

			PlayerForward.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Forward").positive.ToString ();
			PlayerBackward.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Backward").positive.ToString ();
			PlayerRight.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Right").positive.ToString ();
			PlayerLeft.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Player Left").positive.ToString ();
			Sprint.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Sprint").positive.ToString ();

			ShipEnter.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Enter Ship").positive.ToString ();
			ShipAccelerate.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Accelerate").positive.ToString ();
			ShipBrake.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Brake").positive.ToString ();
			ShipLeft.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Left").positive.ToString ();
			ShipRight.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Ship Right").positive.ToString ();

			Use.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Use").positive.ToString ();
			Pause.GetComponentInChildren<Text> ().text = InputManager.GetAxisConfiguration ("Default", "Pause").positive.ToString ();
		}

		void AllUninteractable(){
			PlayerForward.interactable = false;
			PlayerBackward.interactable = false;
			PlayerLeft.interactable = false;
			PlayerRight.interactable = false;
			Sprint.interactable = false;

			ShipEnter.interactable = false;
			ShipAccelerate.interactable = false;
			ShipBrake.interactable = false;
			ShipLeft.interactable = false;
			ShipRight.interactable = false;

			Use.interactable = false;
			Pause.interactable = false;
		}

		bool HandleKeyScan(ScanResult result){
			
			var axis = InputManager.GetAxisConfiguration ("Default" , result.userData.ToString());
			axis.positive = result.key;
			if (result.key != KeyCode.None) {
				isEditing = false;
				Debug.Log ("isnt editing");
			}
			Load ();
			return true;
		}

		public void OnPlayerForward(){
			AllUninteractable ();

			var colors = PlayerForward.colors;
			colors.colorMultiplier = 5f;
			PlayerForward.colors = colors;

			PlayerForward.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Forward";
			settings.timeout = 20f;
			isEditing = true;
			Debug.Log ("isediding");
			InputManager.StartScan(settings, HandleKeyScan);

		}

		public void OnPlayerBackward(){
			AllUninteractable ();
			var colors = PlayerBackward.colors;
			colors.colorMultiplier = 5f;
			PlayerBackward.colors = colors;

			PlayerBackward.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Backward";
			settings.timeout = 20f;
			isEditing = true;
			InputManager.StartScan(settings, HandleKeyScan);

		}

		public void OnPlayerLeft(){
			AllUninteractable ();
			var colors = PlayerLeft.colors;
			colors.colorMultiplier = 5f;
			PlayerLeft.colors = colors;

			PlayerLeft.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Left";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
		}

		public void OnPlayerRight(){
			AllUninteractable ();
			var colors = PlayerRight.colors;
			colors.colorMultiplier = 5f;
			PlayerRight.colors = colors;

			PlayerRight.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Player Right";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
		}

		public void OnSprint(){
			AllUninteractable ();

			var colors = PlayerForward.colors;
			colors.colorMultiplier = 5f;
			Sprint.colors = colors;

			Sprint.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Sprint";
			settings.timeout = 20f;
			isEditing = true;
			Debug.Log ("isediding");
			InputManager.StartScan(settings, HandleKeyScan);

		}




		public void OnShipEnter(){
			AllUninteractable ();
			var colors = ShipEnter.colors;
			colors.colorMultiplier = 5f;
			ShipEnter.colors = colors;

			ShipEnter.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Enter Ship";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;

		}

		public void OnShipAccelerate(){
			AllUninteractable ();
			var colors = ShipAccelerate.colors;
			colors.colorMultiplier = 5f;
			ShipAccelerate.colors = colors;

			ShipAccelerate.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Accelerate";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
		}

		public void OnShipBrake(){
			AllUninteractable ();
			var colors = ShipBrake.colors;
			colors.colorMultiplier = 5f;
			ShipBrake.colors = colors;

			ShipBrake.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Brake";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
		}

		public void OnShipLeft(){
			AllUninteractable ();
			var colors = ShipLeft.colors;
			colors.colorMultiplier = 5f;
			ShipLeft.colors = colors;

			ShipLeft.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Left";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
		}

		public void OnShipRight(){
			AllUninteractable ();
			var colors = ShipRight.colors;
			colors.colorMultiplier = 5f;
			ShipRight.colors = colors;

			ShipRight.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Ship Right";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
		}



		public void OnUse(){
			AllUninteractable ();
			var colors = Use.colors;
			colors.colorMultiplier = 5f;
			Use.colors = colors;

			Use.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.cancelScanButton = "Escape";
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Use";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
		}

		public void OnPause(){
			AllUninteractable ();
			var colors = Pause.colors;
			colors.colorMultiplier = 5f;
			Pause.colors = colors;

			Pause.GetComponentInChildren<Text> ().text = "";
			ScanSettings settings = new ScanSettings ();
			settings.scanFlags = ScanFlags.Key;
			settings.userData = "Pause";
			settings.timeout = 20f;
			InputManager.StartScan(settings, HandleKeyScan);
			isEditing = true;
			Debug.Log ("Is editing");
		}
	}
}
