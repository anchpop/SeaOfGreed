using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

namespace AssemblyCSharp
{

	public class PlayerController : MonoBehaviour {
	    enum states
	    {
	        noState,
	        onLand,
	        swimming,
	        boardedShip,
	        steeringShip,
	    }

	    states state = states.onLand;
	    states newState = states.noState;

	    public float walkSpeed = 2;
	    public float width = .12f;
	    public float height = .12f;

	    public float boardShipRange = 10;
	    public float dockShipRange = 10;

	    public float minDistanceToGrabWheel = .5f;

	    public LayerMask walkRaycastMask;
	    public LayerMask dockRaycastMask;
	    public LayerMask boatRaycastMask;

	    public GameObject boardText;
	    public GameObject dockText;
	    public GameObject wheelText;

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

	    void grabWheel()
	    {
	        newState = states.steeringShip;
			wheelText.SetActive(false);
	        transform.position = shipBorded.GetComponent<ShipController>().wheelMarker.transform.position;
	    }

	    void releaseWheel()
	    {
	        newState = states.boardedShip;
	    }

	    void boardShip(GameObject ship)
	    {
	        newState = states.boardedShip;
	        transform.position = ship.transform.position;
	        transform.SetParent(ship.transform);
	        shipBorded = ship;
			boardText.SetActive (false);
	    }

	    void dockShip(Vector3 position)
	    {
	        shipBorded = null;
	        transform.position = position;
	        transform.SetParent(null);
	        transform.rotation = Quaternion.identity;
	        newState = states.onLand;
	        shipBorded = null;
			dockText.SetActive (false);

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
	        RaycastHit2D x_ray = Physics2D.Raycast(transform.position + xToOffset / 10, xToOffset, width, walkRaycastMask);
	        RaycastHit2D y_ray = Physics2D.Raycast(transform.position + yToOffset / 10, yToOffset, height, walkRaycastMask);
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
	                    boardShip(boatFound.collider.gameObject);
	                }
	            }

	            walkAccordingToUserInput();
	        }
	        if (state == states.boardedShip)
	        {
	            var dockFound = dockSearch(1);
	            if (dockFound) Debug.Log("Dock Found: " + dockFound.transform.position);
	            var distanceToWheel = getDistanceToWheel();
	            if (distanceToWheel > minDistanceToGrabWheel)
	            {
	                wheelText.SetActive(false);
	            }
	            else if (distanceToWheel < minDistanceToGrabWheel)
	            {
	                wheelText.SetActive(true);
					if (Input.GetKeyDown(Keybindings.use))
	                {
	                    grabWheel();
	                }
	            }
	            if (dockFound)
	            {
	                dockText.SetActive(true);
					if (Input.GetKeyDown(Keybindings.enterShip))
	                {
	                    Debug.Log(dockFound.point);
	                    dockShip(dockFound.point);
	                }
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
				if (Input.GetKey(Keybindings.shipRight))
	            {
	                shipController.turn(1);
	            }
				if (Input.GetKey(Keybindings.shipLeft))
	            {
	                shipController.turn(-1);
	            }
				if (Input.GetKeyDown(Keybindings.use))
	            {
	                releaseWheel();
	            }
	        }
	    }
	}
<<<<<<< HEAD
}
=======

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
        RaycastHit2D x_ray = Physics2D.Raycast(transform.position + xToOffset / 10, xToOffset, width, walkRaycastMask);
        RaycastHit2D y_ray = Physics2D.Raycast(transform.position + yToOffset / 10, yToOffset, height, walkRaycastMask);
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
                if (Input.GetKeyDown(KeyCode.V))
                {
                    onLandToBoardedShip(boatFound.collider.gameObject);
                }
            }

            walkAccordingToUserInput();
        }
        if (state == states.boardedShip)
        {
            var dockFound = dockSearch(1);
            if (dockFound) Debug.Log("Dock Found: " + dockFound.transform.position);
            var distanceToWheel = getDistanceToWheel();
            if (distanceToWheel > minDistanceToGrabWheel)
            {
                wheelText.SetActive(false);
            }
            else if (distanceToWheel < minDistanceToGrabWheel)
            {
                wheelText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    boardedShipToSteeringShip();
                }
            }
            if (dockFound)
            {
                dockText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.V))
                {
                    Debug.Log(dockFound.point);
                    boardedShipToOnLand(dockFound.point);
                }
            }

            walkAccordingToUserInput();
        }

        if (state == states.steeringShip)
        {
            var shipController = shipBorded.GetComponent<ShipController>();
            if (Input.GetKey(KeyCode.W))
            {
                shipController.accelerate();
            }
            if (Input.GetKey(KeyCode.S))
            {
                shipController.brake();
            }
            if (Input.GetKey(KeyCode.A))
            {
                shipController.turn(1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                shipController.turn(-1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                steeringShipToBoardedShip();
            }
        }
    }

    // state transitions
    void boardedShipToSteeringShip()
    {
        Assert.IsTrue(state == states.boardedShip);
        newState = states.steeringShip;
        wheelText.SetActive(false);
        transform.position = shipBorded.GetComponent<ShipController>().wheelMarker.transform.position;
    }

    void steeringShipToBoardedShip()
    {
        Assert.IsTrue(state == states.steeringShip);
        newState = states.boardedShip;
    }

    void onLandToBoardedShip(GameObject ship)
    {
        Assert.IsTrue(state == states.onLand);
        newState = states.boardedShip;
        transform.position = ship.transform.position;
        transform.SetParent(ship.transform);
        shipBorded = ship;
        boardText.SetActive(false);
    }

    void boardedShipToOnLand(Vector3 position)
    {
        Assert.IsTrue(state == states.boardedShip);
        shipBorded = null;
        transform.position = position;
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        newState = states.onLand;
        shipBorded = null;
        dockText.SetActive(false);

    }
}
>>>>>>> origin/master
