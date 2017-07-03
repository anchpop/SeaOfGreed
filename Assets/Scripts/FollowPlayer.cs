using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public float maxDistance = 3;
    public float dampTime = 0.15f;
    float zDistFromTarget = -.5f;
    private Vector3 targetOldPosition;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    new Camera camera;


    void Start()
    {
        camera = GetComponent<Camera>();
        //moveCameraToPlayer(0, 0);
        transform.position = target.transform.position;
        targetOldPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        //velocity = (transform.position - oldPosition) * Time.deltaTime;
        if (target)
        {
            moveCameraToPlayer(dampTime, maxDistance);
        }
        targetOldPosition = target.transform.position;
    }

    Vector3 SuperSmoothLerp(Vector3 followerOldPos, Vector3 targetOldPos, Vector3 targetNewPos, float timeElapsed, float lerpRate)
    {
        Vector3 f = followerOldPos - targetOldPos + (targetNewPos - targetOldPos) / (lerpRate * timeElapsed);
        return targetNewPos - (targetNewPos - targetOldPos) / (lerpRate * timeElapsed) + f * Mathf.Exp(-lerpRate * timeElapsed);
    }


    void moveCameraToPlayer(float dampTime, float maxDist)
    {
        Vector3 point = camera.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        transform.position = SuperSmoothLerp(transform.position, targetOldPosition, target.transform.position, Time.deltaTime, dampTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, zDistFromTarget);

        if ((transform.position - destination).magnitude > maxDist)
        {
            var difference = transform.position - destination;
            transform.position = destination + difference.normalized * maxDist;
        }

    }
}