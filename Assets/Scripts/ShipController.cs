using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    Rigidbody2D body;

    public float acceleration = 5;
    public float breakspeed = 2;
    public float maxspeed = 10;
    public float torque = 2;
    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
    }

    private void applyContinuousForce(float speed, Vector3 force, float maxVelocity, float minVelocity = 0)
    {
        Vector2 forceToApply = speed * force;
        Vector2 velocityChange = forceToApply * Time.fixedDeltaTime;
        float velocityInDirection = Vector3.Dot(body.velocity, force.normalized);
        float velocityInDirectionAfterForce = Vector3.Dot(body.velocity + velocityChange, force.normalized);

                
        if (Mathf.Abs(velocityInDirectionAfterForce) < maxVelocity || Mathf.Sign(velocityInDirectionAfterForce) < 0) // if we're moving in the opposite direction of movement
            body.AddForce(forceToApply, ForceMode.Acceleration);
        else if (Mathf.Abs(velocityInDirection) < maxVelocity) // if the speed addition wouldn't make us accelerate past the max run speed
            body.velocity = body.velocity - (Vector2)Vector3.Project(body.velocity, forceToApply) + (Vector2)(force.normalized * maxVelocity);
        //else if (Mathf.Abs(body.velocity.x) <= maxRunVel)       // if we're moving slower than the max run speed already
        // body.velocity = new Vector2(Sign(body.velocity.x) * maxRunVel, body.velocity.y); // bring us straight to the maximum speed  
    }

    // Update is called once per frame
    void Update () { 
        if (Input.GetKey(KeyCode.W)) {
            Debug.Log("Adding force");
            applyContinuousForce(acceleration, transform.up, maxspeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("Adding force");
            applyContinuousForce(breakspeed, -transform.up, 0);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Adding torque");
            body.AddTorque(torque);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Adding torque");
            body.AddTorque(-torque);
        }
    }
}
