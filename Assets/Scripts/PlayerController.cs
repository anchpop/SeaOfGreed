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

		public CharacterDriver driver;

        // Use this for initialization
        void Start () {
		    Keybindings.Load ();
			driver = gameObject.GetComponent<CharacterDriver> ();
	    }    

        void walkAccordingToUserInput()
        {
            var input_x = Input.GetAxisRaw("Horizontal");
            var input_y = Input.GetAxisRaw("Vertical");

            driver.walkInDirection(new Vector3(input_x, input_y));
        }

        void steerShipAccordingToUserInput()
        {
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

        void lookTowardsMouse()
        {
            if (driver.state == states.boardedShip || driver.state == states.onLand || driver.state == states.jumpingToLand || driver.state == states.jumpingToShip)
            {
                var mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z - transform.position.z));
                Vector3 diff = (mousePos - driver.sprite.transform.position);
                driver.lookInDirection(diff);
            }
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
            if (driver.state == states.boardedShip || driver.state == states.onLand)
            {
                walkAccordingToUserInput();
            }
            if (driver.state == states.boardedShip || driver.state == states.onLand || driver.state == states.jumpingToLand || driver.state == states.jumpingToShip)
            {
                lookTowardsMouse();
            }

            if (driver.state == states.onLand && driver.canBoardShip() && Input.GetKeyDown(Keybindings.enterShip))
            {
                driver.boardShipHelper();
            }
            if (driver.state == states.boardedShip && driver.canDockShip() && Input.GetKeyDown(Keybindings.enterShip))
            {
                driver.dockShipHelper();
            }
            if (driver.state == states.boardedShip && driver.canGrabWheel() && Input.GetKeyDown(Keybindings.use))
            {
                driver.grabWheelHelper();
            }
            if (driver.state == states.steeringShip)
            {
                steerShipAccordingToUserInput();
            }

        }




    }
}