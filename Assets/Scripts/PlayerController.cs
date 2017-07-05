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
        states state = states.onLand;
        states newState = states.noState;

        public float walkSpeed = 2;
        public float width = .12f;
        public float height = .12f;

        public float boardShipRange = 10;
        public float dockShipRange = 10;
        public float dockOffset = .5f;
        public float jumpTime = 1;


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

        Animator anim;
    
        GameObject shipBorded;
    
    



        // Use this for initialization
        void Start () {
		    Keybindings.Load ();
            anim = GetComponent<Animator>();
	    }

        RaycastHit2D boatSearch(int iterations)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, boardShipRange, boatRaycastMask);
            return hit;
        }
        RaycastHit2D dockSearch(int iterations)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, boardShipRange, dockRaycastMask);
            return hit;
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

        // Update is called once per frame
        void Update () {
            if (newState != states.noState)
            {
                state = newState;
                newState = states.noState;
            }

            if (state == states.onLand)
            {
                RaycastHit2D boatFound = boatSearch(1);
                if (!boatFound)
                {
                    boardText.SetActive(false);
                }
                if (boatFound)
                {
                    boardText.SetActive(true);
                    if (Input.GetKeyDown(Keybindings.enterShip))
                    {
                        onLandToBoardedShip(boatFound.collider.gameObject);
                    }
                }

                walkAccordingToUserInput();
            }
            if (state == states.boardedShip)
            {
                var dockFound = dockSearch(1);
            
                var distanceToWheel = getDistanceToWheel();
                if (distanceToWheel > minDistanceToGrabWheel)
                {
                    wheelText.SetActive(false);
                }
                else if (distanceToWheel < minDistanceToGrabWheel)
                {
                    if (Input.GetKeyDown(Keybindings.use))
                    {
                        boardedShipToSteeringShip();
                    }
                }
                if (dockFound)
                {
                    if (Input.GetKeyDown(Keybindings.enterShip))
                    {
                        boardedShipToOnLand(dockFound.point);
                    }
                }

                if (distanceToWheel < minDistanceToGrabWheel)
                {
                    wheelText.SetActive(true);
                    dockText.SetActive(false);
                }   
                else
                {
                    wheelText.SetActive(false);
                    if (dockFound)
                        dockText.SetActive(true);
                    else
                        dockText.SetActive(false);
                }

                walkAccordingToUserInput();
            }

            if (state == states.steeringShip)
            {
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
        }

        // state transitions
        void boardedShipToOnLand(Vector3 closestDock) // this was a test of the animations, didn't work lol
        {

            var fromLocation = transform.position;
            // get the location to jump to by extending the line between fromLocation and closestDock
            var jumpVector = closestDock - fromLocation;
            var toLocation = fromLocation + (jumpVector.normalized * (jumpVector.magnitude + dockOffset));

            newState = states.jumpingToLand;

            var seq = LeanTween.sequence();
            seq.append(LeanTween.value(gameObject, pos => transform.position = pos, fromLocation, toLocation, jumpTime).setEase(LeanTweenType.easeInOutExpo));
            seq.append(() => jumpingToLandToOnLand());

        }

        void onLandToBoardedShip(GameObject ship)
        {
            var fromLocation = transform.position;
            var toLocation = ship.transform.position;
            newState = states.jumpingToShip;

            var seq = LeanTween.sequence();
            seq.append(LeanTween.value(gameObject, pos => transform.position = pos, fromLocation, toLocation, jumpTime).setEase(LeanTweenType.easeInOutExpo));
            seq.append(() => jumpingToShipToBoardedShip(ship));
        }

        void jumpingToShipToBoardedShip(GameObject ship)
        {
            Assert.IsTrue(state == states.jumpingToShip);
            transform.rotation = ship.transform.rotation;
            newState = states.boardedShip;
            transform.position = ship.transform.position;
            transform.SetParent(ship.transform);
            shipBorded = ship;
            boardText.SetActive(false);
        }

        void boardedShipToSteeringShip()
        {
            Assert.IsTrue(state == states.boardedShip);
            newState = states.steeringShip;
            wheelText.SetActive(false);
            transform.position = shipBorded.GetComponent<ShipController>().wheelMarker.transform.position;
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
            dockText.SetActive(false);
        }
    }
}