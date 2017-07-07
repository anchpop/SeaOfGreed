using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using TeamUtility.IO;

namespace SeaOfGreed{
    public class PlayerController : MonoBehaviour {
        
        enum helpText
        {
            boardText,
            dockText,
            wheelText,
            none,
        }

        helpText helpTextToDisplay = helpText.none;

        public GameObject boardText;
        public GameObject dockText;
        public GameObject wheelText;

        public Camera mainCamera;
        public float walkingCameraSize = 4;
        public float interactingCameraSize = 6;
        public float cameraEaseTime = 2;

		public CharacterDriver driver;

        // Use this for initialization
        void Start () {
			driver = gameObject.GetComponent<CharacterDriver> ();
	    }    

        void walkAccordingToUserInput()
        {
			var input_x_right = InputManager.GetButton ("Player Right") ? 1 : 0;
			var input_x_left = InputManager.GetButton ("Player Left") ? 1 : 0;
			var input_y_forward = InputManager.GetButton("Player Forward") ? 1 : 0;
			var input_y_backward = InputManager.GetButton ("Player Backward") ? 1 : 0;
			var input_x = input_x_right - input_x_left;
			var input_y = input_y_forward - input_y_backward;

            driver.walkInDirection(new Vector3(input_x, input_y));
        }

        void steerShipAccordingToUserInput()
        {
            driver.sprite.transform.localRotation = Quaternion.identity;
            var shipController = driver.shipBorded.GetComponent<ShipController>();
			if (InputManager.GetButton("Ship Accelerate"))
            {
                shipController.accelerate();
            }
			if (InputManager.GetButton("Ship Brake"))
            {
                shipController.brake();
            }
			if (InputManager.GetButton("Ship Left"))
            {
                shipController.turn(1);
            }
			if (InputManager.GetButton("Ship Right"))
            {
                shipController.turn(-1);
            }
			if (InputManager.GetButtonDown("Use"))
            {
				//Debug.Log("WantsToSteeringShiptoBoardedShip");
                driver.steeringShipToBoardedShip();
            }
        }

        void lookTowardsMouse()
        {
            if (driver.state == states.boardedShip || driver.state == states.onLand || driver.state == states.jumpingToLand || driver.state == states.jumpingToShip)
            {
                var mousePos = mainCamera.ScreenToWorldPoint(new Vector3(InputManager.mousePosition.x, InputManager.mousePosition.y, mainCamera.transform.position.z - transform.position.z));
                Vector3 diff = (mousePos - driver.sprite.transform.position);
                driver.lookInDirection(diff);
            }
        }

        void displayHelpText()
        {
            boardText.SetActive(false);
            dockText.SetActive(false);
            wheelText.SetActive(false);

            if (driver.state == states.boardedShip && driver.canGrabWheel())
                wheelText.SetActive(true);
            else if (driver.state == states.onLand && driver.canBoardShip())
                boardText.SetActive(true);
            else if (driver.state == states.boardedShip && driver.canDockShip())
                dockText.SetActive(true);
        }

        // Update is called once per frame
        void FixedUpdate () {
            if (driver.state == states.boardedShip || driver.state == states.onLand)
            {
                walkAccordingToUserInput();
            }
                displayHelpText();

			if (driver.state == states.onLand && driver.canBoardShip() && InputManager.GetButtonDown("Enter Ship"))
            {
                driver.boardShipHelper();
            }
				if (driver.state == states.boardedShip && driver.canDockShip() && InputManager.GetButtonDown("Enter Ship"))
            {
                driver.dockShipHelper();
            }
			if (driver.state == states.boardedShip && driver.canGrabWheel() && InputManager.GetButtonDown("Use"))
            {
                driver.grabWheelHelper();
            }
            if (driver.state == states.steeringShip)
            {
                steerShipAccordingToUserInput();
            }
			if (driver.state == states.boardedShip || driver.state == states.onLand || driver.state == states.jumpingToLand || driver.state == states.jumpingToShip)
			{
				lookTowardsMouse();
			}
				
        }


    }
}