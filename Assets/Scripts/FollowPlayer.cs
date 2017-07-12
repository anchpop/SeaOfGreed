using UnityEngine;

namespace SeaOfGreed {

    public class FollowPlayer : MonoBehaviour
    {
        public float maxDistance = 3;
        public float dampTime = 0.15f;
        private float zDistFromTarget = -.5f;
        private Vector3 targetOldPosition;
        private Vector3 velocity = Vector3.zero;
        public Transform target = null;
        private PlayerController playerController;
        private new Camera camera;

        private void Start()
        {
            camera = GetComponent<Camera>();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().mainCameraBlackout = gameObject.GetComponent<CameraBlackout>();
            //moveCameraToPlayer(0, 0);
            transform.position = target.transform.position;
            targetOldPosition = transform.position;
            playerController = target.GetComponent<PlayerController>();
            playerController.driver.StateChanged += driver_StateChanged;
        }

        // Update is called once per frame
        private void Update()
        {
            if (target)
            {
                if (playerController.driver.state == states.steeringShip)
                {
                    camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, playerController.interactingCameraSize, .1f * Time.deltaTime);
                }
                else if (playerController.driver.state == states.onLand || playerController.driver.state == states.boardedShip)
                {
                    //camera.orthographicSize = 4.5f;
                    camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, playerController.walkingCameraSize, .3f * Time.deltaTime);
                }
                moveCameraToPlayer(dampTime, maxDistance);
            }
            targetOldPosition = target.transform.position;
        }

        private Vector3 SuperSmoothLerp(Vector3 followerOldPos, Vector3 targetOldPos, Vector3 targetNewPos, float timeElapsed, float lerpRate)
        {
            Vector3 f = followerOldPos - targetOldPos + (targetNewPos - targetOldPos) / (lerpRate * timeElapsed);
            return targetNewPos - (targetNewPos - targetOldPos) / (lerpRate * timeElapsed) + f * Mathf.Exp(-lerpRate * timeElapsed);
        }

        private void moveCameraToPlayer(float dampTime, float maxDist)
        {
            Vector3 point = camera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            transform.position = SuperSmoothLerp(transform.position, targetOldPosition, target.transform.position, Time.deltaTime == 0 ? .2f : Time.deltaTime, dampTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zDistFromTarget);
            //transform.rotation = target.rotation;

            if ((transform.position - destination).magnitude > maxDist)
            {
                var difference = transform.position - destination;
                transform.position = destination + difference.normalized * maxDist;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, 1.5f * Time.deltaTime);
        }

        public void setCameraToPlayer()
        {
            transform.position = target.transform.position;
            targetOldPosition = transform.position;
        }

        private void driver_StateChanged(CharacterDriver sender, StateChangedEventArgs e)
        {
            if (sender.controller != null)
            {
                if (e.beforeState == states.boardedShip && e.afterState == states.steeringShip)
                {
                    LeanTween.cancel(sender.controller.mainCamera.gameObject);
                    LeanTween.value(sender.controller.mainCamera.gameObject, val => sender.controller.mainCamera.orthographicSize = val, sender.controller.mainCamera.orthographicSize, sender.controller.interactingCameraSize, sender.controller.cameraEaseTime).setEase(LeanTweenType.easeInOutQuad);
                }
                if (e.beforeState == states.steeringShip && e.afterState == states.boardedShip)
                {
                    LeanTween.cancel(sender.controller.mainCamera.gameObject);
                    LeanTween.value(sender.controller.mainCamera.gameObject, val => sender.controller.mainCamera.orthographicSize = val, sender.controller.mainCamera.orthographicSize, sender.controller.walkingCameraSize, sender.controller.cameraEaseTime).setEase(LeanTweenType.easeInOutQuad);
                }
            }
        }
    }
}