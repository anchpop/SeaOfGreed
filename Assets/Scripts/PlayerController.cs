using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

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

		public PlayerDriver driver;

        // Use this for initialization
        void Start () {
		    Keybindings.Load ();
			driver = gameObject.GetComponent<PlayerDriver> ();
	    }    

        void walkAccordingToUserInput()
        {
            var input_x = Input.GetAxisRaw("Horizontal");
            var input_y = Input.GetAxisRaw("Vertical");

            driver.walkInDirection(new Vector3(input_x, input_y));
            
        }

        void displayHelpText()
        {
            boardText.SetActive(false);
            dockText.SetActive(false);
            wheelText.SetActive(false);

            if (helpTextToDisplay == helpText.wheelText)
                wheelText.SetActive(true);
            else if (helpTextToDisplay == helpText.boardText)
                boardText.SetActive(true);
            else if (helpTextToDisplay == helpText.dockText)
                dockText.SetActive(true);
        }

        // Update is called once per frame
        void Update () {
			Debug.Log (driver.state);
            if (driver.newState != states.noState)
            {
                driver.state = driver.newState;
                driver.newState = states.noState;
            }
            helpTextToDisplay = helpText.none;

            if (driver.state == states.onLand)
            {
                RaycastHit2D boatFound = driver.boatSearch(12);
                if (boatFound)
                {
                    helpTextToDisplay = helpText.boardText;
                    if (Input.GetKeyDown(Keybindings.enterShip))
                    {
                        driver.onLandToBoardedShip(boatFound.collider.gameObject);
                    }
                }

                walkAccordingToUserInput();
            }
            if (driver.state == states.boardedShip)
            {
				var dockFound = driver.dockSearch(12);
            
				var distanceToWheel = driver.getDistanceToWheel();
                if (dockFound)
                {
                    helpTextToDisplay = helpText.dockText;
                    if (Input.GetKeyDown(Keybindings.enterShip))
						driver.boardedShipToOnLand(dockFound.point);
                }
				if (distanceToWheel > driver.minDistanceToGrabWheel)
                {
					wheelText.SetActive (false);
                }
				else if (distanceToWheel < driver.minDistanceToGrabWheel)
                {
                    helpTextToDisplay = helpText.wheelText;
                    if (Input.GetKeyDown(Keybindings.use))
						driver.boardedShipToSteeringShip();
                }

                

                walkAccordingToUserInput();
            }
            else
            {
				wheelText.SetActive (false);
				dockText.SetActive (false);
            }

			if (driver.state == states.steeringShip)
            {
				//sprite.transform.rotation = shipBorded.transform.rotation;
				driver.sprite.transform.localRotation = Quaternion.identity;
				var shipController = driver.shipBorded.GetComponent<ShipController>();
                if (Input.GetKey(Keybindings.shipForward))
                {
                    shipController.accelerate();
                }
                if (Input.GetKey(Keybindings.shipReverse))
                {
                    shipController.brake();
                }
                if (Input.GetKey(Keybindings.shipLeft))
                {
                    shipController.turn(1);
                }
                if (Input.GetKey(Keybindings.shipRight))
                {
                    shipController.turn(-1);
                }
                if (Input.GetKeyDown(Keybindings.use))
                {
					driver.steeringShipToBoardedShip();
                }
            }
			if (driver.state == states.boardedShip || driver.state == states.onLand || driver.state == states.jumpingToLand || driver.state == states.jumpingToShip) {
				//var mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				//transform.up = Input.mousePosition - transform.position;
				//Vector3 diff = sprite.transform.InverseTransformPoint(Input.mousePosition);
				var mousePos = mainCamera.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y, mainCamera.transform.position.z-transform.position.z));
				Vector3 diff = (mousePos - driver.sprite.transform.position); 
				var tan = Mathf.Atan2 ( diff.x, diff.y );
				driver.sprite.transform.rotation = Quaternion.Euler(0f, 0f, tan * -Mathf.Rad2Deg );

			}

            displayHelpText();
        }




    }
}