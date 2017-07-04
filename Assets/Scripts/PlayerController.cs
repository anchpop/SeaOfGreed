using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float walkSpeed = 2;
    public float width = .12f;
    public float height = .12f;

    public float boardShipRange = 10;
    public float dockShipRange = 10;

    bool boarded = false;
    GameObject shipBorded;

    public LayerMask walkRaycastMask;
    public LayerMask dockRaycastMask;
    public LayerMask boatRaycastMask;

    // Use this for initialization
    void Start () {
	}

    RaycastHit2D boatSearch(int iterations)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, boardShipRange, boatRaycastMask);
        return hit;
    }

    float getDistanceToWheel()
    {
        return Vector3.Distance(transform.position, shipBorded.GetComponent<ShipController>().wheelMarker.transform.position);
    }

    void boardShip(GameObject ship)
    {
        transform.position = ship.transform.position;
        transform.SetParent(ship.transform);
        boarded = true;
        shipBorded = ship;
    }

    // Update is called once per frame
    void Update () {

        RaycastHit2D boatFound = boatSearch(1);
        if (!shipBorded && boatFound)
        {
            Debug.Log("boatFound");
            if (Input.GetKeyDown(KeyCode.V))
            {
                boardShip(boatFound.collider.gameObject);
            }
        }
        if (shipBorded)
        {
            getDistanceToWheel();
        }

        var input_x = Input.GetAxisRaw("Horizontal"); 
        var input_y = Input.GetAxisRaw("Vertical");

        
        bool isWalking = (input_x != 0) || (input_y != 0);

        var xToOffset = transform.right * input_x;
        var yToOffset = transform.up * input_y;

        // TODO - shoot 2 rays for each dir, one for each corner, according to Sprite.bounds
        // This way the player won't be able to slide past some walls
        RaycastHit2D x_ray = Physics2D.Raycast(transform.position + xToOffset/10, xToOffset, width, walkRaycastMask); 
        RaycastHit2D y_ray = Physics2D.Raycast(transform.position + yToOffset/10, yToOffset, height, walkRaycastMask);
        //Debug.DrawRay(transform.position, xToOffset/50  , Color.green);
        var xOffset = (x_ray) ? xToOffset : Vector3.zero;
        var yOffset = (y_ray) ? yToOffset : Vector3.zero;


        if (isWalking)
            transform.position += ((xOffset ) + (yOffset)).normalized * walkSpeed * Time.deltaTime;
    }
}
