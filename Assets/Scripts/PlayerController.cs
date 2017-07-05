using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

namespace SeaOfGreed{
    public class PlayerController : MonoBehaviour {
        enum states
        {
            noState,
            onLand,
            jumpingToLand,
            swimming,
            boardedShip,
            jumpingToShip,
            steeringShip,
        }
        enum helpText
        {
            boardText,
            dockText,
            wheelText,
            none,
        }
        helpText helpTextToDisplay = helpText.none;
        states state = states.onLand;
        states newState = states.noState;

        public float walkSpeed = 2;
        public float width = .12f;
        public float height = .12f;

        public float boardShipRange = 10;
        public float dockShipRange = 10;
        public float dockOffset = .5f;
        public float jumpTime = 1;
        public float jumpScale = 3;


        public float minDistanceToGrabWheel = .5f;

        public LayerMask groundRaycastMask;
        public LayerMask dockRaycastMask;
        public LayerMask boatRaycastMask;

        public GameObject boardText;
        public GameObject dockText;
        public GameObject wheelText;

        public Camera mainCamera;
        public float walkingCameraSize = 4;
        public float interactingCameraSize = 6;
        public float cameraEaseTime = 2;

		public GameObject sprite;

        Animator anim;
    
        GameObject shipBorded;
    
    



        // Use this for initialization
        void Start () {
		    Keybindings.Load ();
            anim = GetComponent<Animator>();
	    }

        RaycastHit2D raysearch(Vector3 position, float range, int iterations, LayerMask mask)
        {
            RaycastHit2D shortestHit = Physics2D.Raycast(position, Vector2.right.Rotate(360 * 1 / iterations), range, mask);
            for (int iteration = 1; iteration < iterations;  iteration++)
            {
                RaycastHit2D newhit = Physics2D.Raycast(position, Vector2.right.Rotate(360 * iteration/iterations), range, mask);
                if (newhit && (shortestHit.distance > newhit.distance || shortestHit.distance==0)) shortestHit = newhit;
            }
            return shortestHit;
        }

        RaycastHit2D boatSearch(int iterations)
        {
            return raysearch(transform.position, boardShipRange, iterations, boatRaycastMask);
        }
        RaycastHit2D dockSearch(int iterations)
        {
            return raysearch(transform.position, dockShipRange, iterations, dockRaycastMask);
        }

        float getDistanceToWheel()
        {
            return Vector3.Distance(transform.position, shipBorded.GetComponent<ShipController>().wheelMarker.transform.position);
        }

        void walkAccordingToUserInput()
        {
            var input_x = Input.GetAxisRaw("Horizontal");
            var input_y = Input.GetAxisRaw("Vertical");


            bool isWalking = (input_x != 0) || (input_y != 0);

            var xToOffset = transform.right * input_x;
            var yToOffset = transform.up * input_y;

            // TODO - shoot 2 rays for each dir, one for each corner, according to Sprite.bounds
            // This way the player won't be able to slide past some walls
            RaycastHit2D x_ray = Physics2D.Raycast(transform.position + xToOffset / 10, xToOffset, width, (state == states.onLand) ? groundRaycastMask : boatRaycastMask);
            RaycastHit2D y_ray = Physics2D.Raycast(transform.position + yToOffset / 10, yToOffset, height, (state == states.onLand) ? groundRaycastMask : boatRaycastMask);
            //Debug.DrawRay(transform.position, xToOffset/50  , Color.green);
            var xOffset = (x_ray) ? xToOffset : Vector3.zero;
            var yOffset = (y_ray) ? yToOffset : Vector3.zero;


            if (isWalking)
                transform.position += ((xOffset) + (yOffset)).normalized * walkSpeed * Time.deltaTime;
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
            if (newState != states.noState)
            {
                state = newState;
                newState = states.noState;
            }
            helpTextToDisplay = helpText.none;

            if (state == states.onLand)
            {
                RaycastHit2D boatFound = boatSearch(12);
                if (boatFound)
                {
                    helpTextToDisplay = helpText.boardText;
                    if (Input.GetKeyDown(Keybindings.enterShip))
                    {
                        onLandToBoardedShip(boatFound.collider.gameObject);
                    }
                }

                walkAccordingToUserInput();
            }
            if (state == states.boardedShip)
            {
                var dockFound = dockSearch(12);
            
                var distanceToWheel = getDistanceToWheel();
                if (dockFound)
                {
                    helpTextToDisplay = helpText.dockText;
                    if (Input.GetKeyDown(Keybindings.enterShip))
                        boardedShipToOnLand(dockFound.point);
                }
                if (distanceToWheel > minDistanceToGrabWheel)
                {
					wheelText.SetActive (false);
                }
                else if (distanceToWheel < minDistanceToGrabWheel)
                {
                    helpTextToDisplay = helpText.wheelText;
                    if (Input.GetKeyDown(Keybindings.use))
                        boardedShipToSteeringShip();
                }

                

                walkAccordingToUserInput();
            }
            else
            {
				wheelText.SetActive (false);
				dockText.SetActive (false);
            }

            if (state == states.steeringShip)
            {
				//sprite.transform.rotation = shipBorded.transform.rotation;
				sprite.transform.localRotation = Quaternion.identity;
                var shipController = shipBorded.GetComponent<ShipController>();
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
                    steeringShipToBoardedShip();
                }
            }
			if (state == states.boardedShip || state == states.onLand || state == states.jumpingToLand || state == states.jumpingToShip) {
				//var mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				//transform.up = Input.mousePosition - transform.position;
				//Vector3 diff = sprite.transform.InverseTransformPoint(Input.mousePosition);
				var mousePos = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y, Camera.main.transform.position.z-transform.position.z));
				Vector3 diff = (mousePos - sprite.transform.position); 
				var tan = Mathf.Atan2 ( diff.x, diff.y );
				sprite.transform.rotation = Quaternion.Euler(0f, 0f, tan * -Mathf.Rad2Deg );

			}

            displayHelpText();
        }

        // state transitions
        void boardedShipToOnLand(Vector3 closestDock) // this was a test of the animations, didn't work lol
        {

            var fromLocation = transform.position;
            // get the location to jump to by extending the line between fromLocation and closestDock
            var jumpVector = closestDock - fromLocation;
            var toLocation = fromLocation + (jumpVector.normalized * (jumpVector.magnitude + dockOffset));

			var fromRotation = shipBorded.transform.rotation;

            newState = states.jumpingToLand;


            var originalScale = transform.localScale;

            // scale the character so they look like they're jumping
            LeanTween.value(gameObject, (time) => { transform.localScale = new Vector3(originalScale.x + myMath.parabolicScaleCalc(time, jumpScale), originalScale.y + myMath.parabolicScaleCalc(time, jumpScale), originalScale.z); }, -1, 1, jumpTime).setEase(LeanTweenType.linear);

            // sequence object allows us to put tweens in a sequence - in this case it's calling a function that changes the state at the end of the tween
            var seq = LeanTween.sequence();
            // move the character to their destination
            seq.append(LeanTween.value(gameObject, (pos) => {transform.position = pos;}, fromLocation, toLocation, jumpTime).setEase(LeanTweenType.linear));
            //LeanTween.value(gameObject, (rotz) => {transform.rotation.Set(transform.rotation.x, transform.rotation.y, rotz, transform.rotation.w);}, fromRotation.z, Quaternion.identity.z, jumpTime).setEase(LeanTweenType.easeInOutExpo);
            seq.append(jumpingToLandToOnLand);

			

        }

        void onLandToBoardedShip(GameObject ship)
        {
            var fromLocation = transform.position;
            var toLocation = ship.GetComponent<ShipController>().getClosestBoardingPoint(transform.position);

            var originalScale = transform.localScale;


            newState = states.jumpingToShip;

            // scale the character so they look like they're jumping
            LeanTween.value(gameObject, (time) => { transform.localScale = new Vector3(originalScale.x + myMath.parabolicScaleCalc(time, jumpScale), originalScale.y + myMath.parabolicScaleCalc(time, jumpScale), originalScale.z); }, -1, 1, jumpTime).setEase(LeanTweenType.linear);
            
            // sequence object allows us to put tweens in a sequence - in this case it's calling a function that changes the state at the end of the tween
            var seq = LeanTween.sequence(); LeanTween.value(gameObject, (time) => { transform.localScale = new Vector3(originalScale.x + myMath.parabolicScaleCalc(time, jumpScale), originalScale.y + myMath.parabolicScaleCalc(time, jumpScale), originalScale.z); }, -1, 1, jumpTime).setEase(LeanTweenType.linear);
            // move the character to their destination
            seq.append(LeanTween.value(gameObject, pos => transform.position = pos, fromLocation, toLocation, jumpTime).setEase(LeanTweenType.linear));

            seq.append(() => jumpingToShipToBoardedShip(ship, toLocation));
        }

        void jumpingToShipToBoardedShip(GameObject ship, Vector3 location)
        {
            Assert.IsTrue(state == states.jumpingToShip);
            transform.rotation = ship.transform.rotation;
            newState = states.boardedShip;
            transform.position = location;
            transform.SetParent(ship.transform);
            shipBorded = ship;
        }

        void boardedShipToSteeringShip()
        {
            Assert.IsTrue(state == states.boardedShip);
            newState = states.steeringShip;
			helpTextToDisplay = helpText.none;
            transform.position = shipBorded.GetComponent<ShipController>().wheelMarker.transform.position;
			//sprite.transform.localRotation = shipBorded.GetComponent<ShipController>().wheelMarker.transform.rotation;
            LeanTween.cancel(mainCamera.gameObject);
            LeanTween.value(mainCamera.gameObject, val => mainCamera.orthographicSize = val, mainCamera.orthographicSize, interactingCameraSize, cameraEaseTime).setEase(LeanTweenType.easeInOutQuad);
            //LeanTween.value(walkingCameraSize, interactingCameraSize)
        }

        void steeringShipToBoardedShip()
        {
            LeanTween.cancel(mainCamera.gameObject);
            LeanTween.value(mainCamera.gameObject, val => mainCamera.orthographicSize = val, mainCamera.orthographicSize, walkingCameraSize, cameraEaseTime).setEase(LeanTweenType.easeInOutQuad);

            Assert.IsTrue(state == states.steeringShip);
            newState = states.boardedShip;
        }


        void jumpingToLandToOnLand()
        {
            Assert.IsTrue(state == states.jumpingToLand);
			shipBorded = null;
			transform.SetParent(null);
			transform.rotation = Quaternion.identity;
            newState = states.onLand;
            shipBorded = null;

        }


    }
}