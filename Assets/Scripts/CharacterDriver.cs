using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SeaOfGreed{
	public class CharacterDriver : MonoBehaviour {

		internal states state = states.onLand;
		internal states newState = states.noState;

		public float boardShipRange = 10;
		public float dockShipRange = 10;
		public float dockOffset = .5f;
		public float jumpSpeed = 10;
		public float jumpScale = 3;
		public float maxDistanceToGrabWheel = .5f;

		public float walkSpeed = 2;
		public float width = .12f;
		public float height = .12f;

		public LayerMask groundRaycastMask;
		public LayerMask dockRaycastMask;
		public LayerMask boatRaycastMask;
        public LayerMask roomTransitionRaycastMask;

        public GameObject sprite;

		Animator anim;

		internal GameObject shipBorded;
		internal PlayerController controller;

        public bool canSwitchIntoRooms = true;


		public delegate void StateChangedEventHandler(CharacterDriver sender, StateChangedEventArgs e);
		public event StateChangedEventHandler StateChanged;


		// Use this for initialization
		void Start () {
			anim = GetComponent<Animator>();
			controller = gameObject.GetComponent<PlayerController> ();
		}
		
		// Update is called once per frame
		void Update () {
            //Debug.Log(state);
            if (newState != states.noState)
            {
                state = newState;
                newState = states.noState;
            }
            
        }

		internal RaycastHit2D raysearch(Vector3 position, float range, int iterations, LayerMask mask)
		{
			RaycastHit2D shortestHit = Physics2D.Raycast(position, Vector2.right.Rotate(360 * 1 / iterations), range, mask);
			for (int iteration = 1; iteration < iterations;  iteration++)
			{
				RaycastHit2D newhit = Physics2D.Raycast(position, Vector2.right.Rotate(360 * iteration/iterations), range, mask);
				if (newhit && (shortestHit.distance > newhit.distance || shortestHit.distance==0)) shortestHit = newhit;
			}
			return shortestHit;
		}

		internal RaycastHit2D boatSearch(int iterations)
		{
			return raysearch(transform.position, boardShipRange, iterations, boatRaycastMask);
		}
		internal RaycastHit2D dockSearch(int iterations)
		{
			return raysearch(transform.position, dockShipRange, iterations, dockRaycastMask);
		}

        public void boardShipHelper()
        {
            Assert.IsTrue(state == states.onLand);
            onLandToBoardedShip(boatSearch(12).collider.gameObject);
        }
        public bool canBoardShip()
        {
            Assert.IsTrue(state == states.onLand);
            return boatSearch(12);
        }

        public void dockShipHelper()
        {
            Assert.IsTrue(state == states.boardedShip);
            boardedShipToOnLand(dockSearch(12).point);
        }
        public bool canDockShip()
        {
            Assert.IsTrue(state == states.boardedShip);
            return dockSearch(12);
        }
        public void grabWheelHelper()
        {
            Assert.IsTrue(state == states.boardedShip);
            boardedShipToSteeringShip();
        }
        public bool canGrabWheel()
		{
            Assert.IsTrue(state == states.boardedShip);
            float distanceToWheel = Vector3.Distance(transform.position, shipBorded.GetComponent<ShipController>().wheelMarker.transform.position);
            bool closeEnoughToGrabWheel = distanceToWheel <= maxDistanceToGrabWheel;

            return closeEnoughToGrabWheel;
		}


        public void switchIntoRoom(RaycastHit2D roomswitchRaycast)
        {
            var polyline = roomswitchRaycast.collider.gameObject.GetComponent<Tiled2Unity.PolylineObject>();
            Debug.Log("Switch into " + polyline.name);
        }

        public void lookInDirection(Vector3 direction)
        {
            
            var tan = Mathf.Atan2(direction.x, direction.y);
            sprite.transform.rotation = Quaternion.Euler(0f, 0f, tan * -Mathf.Rad2Deg);

        }


        public void walkInDirection(Vector3 direction)
        {
            bool isWalking = (direction.x != 0) || (direction.y != 0);
            if (isWalking)
            {

                var xToOffset = transform.right * direction.x;
                var yToOffset = transform.up * direction.y;

                // TODO - shoot 2 rays for each dir, one for each corner, according to Sprite.bounds
                // This way the player won't be able to slide past some walls
                RaycastHit2D x_ray = Physics2D.Raycast(transform.position + xToOffset / 10, xToOffset, width, (state == states.onLand) ? groundRaycastMask : boatRaycastMask);
                RaycastHit2D y_ray = Physics2D.Raycast(transform.position + yToOffset / 10, yToOffset, height, (state == states.onLand) ? groundRaycastMask : boatRaycastMask);
                //Debug.DrawRay(transform.position, xToOffset/50  , Color.green);
                var xOffset = (x_ray) ? xToOffset : Vector3.zero;
                var yOffset = (y_ray) ? yToOffset : Vector3.zero;

                transform.position += ((xOffset) + (yOffset)).normalized * walkSpeed * Time.deltaTime;

                if (canSwitchIntoRooms)
                {
                    RaycastHit2D roomswitch_x_ray = Physics2D.Raycast(transform.position + xToOffset / 10, xToOffset, width, roomTransitionRaycastMask);
                    RaycastHit2D roomswitch_y_ray = Physics2D.Raycast(transform.position + yToOffset / 10, yToOffset, height, roomTransitionRaycastMask);
                
                    if (roomswitch_x_ray)
                        switchIntoRoom(roomswitch_x_ray);
                    else if (roomswitch_y_ray)
                        switchIntoRoom(roomswitch_y_ray);
                }

            }
        }

		// state transitions
		public void boardedShipToOnLand(Vector3 closestDock) // this was a test of the animations, didn't work lol
		{
			var fromLocation = transform.position;
			// get the location to jump to by extending the line between fromLocation and closestDock
			var jumpVector = closestDock - fromLocation;
			var toLocation = fromLocation + (jumpVector.normalized * (jumpVector.magnitude + dockOffset));
            var jumpDistance = (fromLocation - toLocation).magnitude;
            var jumpTime = jumpDistance / jumpSpeed;


            var fromRotation = shipBorded.transform.rotation;

			newState = states.jumpingToLand;


			var originalScale = transform.localScale;

            // scale the character so they look like they're jumping
            LeanTween.value(gameObject,
                (time) => {
                    transform.localScale = new Vector3(
                    originalScale.x + myMath.parabolicScaleCalc(time, jumpScale * jumpTime),
                    originalScale.y + myMath.parabolicScaleCalc(time, jumpScale * jumpTime),
                    originalScale.z);
                },
                -1, 1, jumpTime)
                .setEase(LeanTweenType.linear);

            // sequence object allows us to put tweens in a sequence - in this case it's calling a function that changes the state at the end of the tween
            var seq = LeanTween.sequence();
			// move the character to their destination
			seq.append(LeanTween.value(gameObject, 
                (pos) => transform.position = pos, 
                fromLocation, toLocation, jumpTime)
                .setEase(LeanTweenType.linear));
			//LeanTween.value(gameObject, (rotz) => {transform.rotation.Set(transform.rotation.x, transform.rotation.y, rotz, transform.rotation.w);}, fromRotation.z, Quaternion.identity.z, jumpTime).setEase(LeanTweenType.easeInOutExpo);
			seq.append(jumpingToLandToOnLand);

		}

		public void onLandToBoardedShip(GameObject ship)
		{
			var fromLocation = transform.position;
			var toLocation = ship.GetComponent<ShipController>().getClosestBoardingPoint(transform.position);
            var jumpDistance = (fromLocation - toLocation).magnitude;
            var jumpTime = jumpDistance / jumpSpeed;

            var originalScale = transform.localScale;


			newState = states.jumpingToShip;

			// scale the character so they look like they're jumping
			LeanTween.value(gameObject, 
                (time) => {
                    transform.localScale = new Vector3(
                        originalScale.x + myMath.parabolicScaleCalc(time, jumpScale), 
                        originalScale.y + myMath.parabolicScaleCalc(time, jumpScale), 
                        originalScale.z); }, 
                -1, 1, jumpTime)
                .setEase(LeanTweenType.linear);

			// sequence object allows us to put tweens in a sequence - in this case it's calling a function that changes the state at the end of the tween
			var seq = LeanTween.sequence();

            //LeanTween.value(gameObject, (time) => { transform.localScale = new Vector3(originalScale.x + myMath.parabolicScaleCalc(time, jumpScale), originalScale.y + myMath.parabolicScaleCalc(time, jumpScale), originalScale.z); }, -1, 1, jumpTime).setEase(LeanTweenType.linear);
			// move the character to their destination
			seq.append(LeanTween.value(gameObject, pos => transform.position = pos, fromLocation, toLocation, jumpTime).setEase(LeanTweenType.linear));

			seq.append(() => jumpingToShipToBoardedShip(ship, toLocation));
		}

		public void jumpingToShipToBoardedShip(GameObject ship, Vector3 location)
		{
			Assert.IsTrue(state == states.jumpingToShip);
			transform.rotation = ship.transform.rotation;
			newState = states.boardedShip;
			transform.position = location;
			transform.SetParent(ship.transform);
			shipBorded = ship;
		}

		public void boardedShipToSteeringShip()
		{
			Assert.IsTrue(state == states.boardedShip);
			newState = states.steeringShip;
			transform.position = shipBorded.GetComponent<ShipController>().wheelMarker.transform.position;
			//sprite.transform.localRotation = shipBorded.GetComponent<ShipController>().wheelMarker.transform.rotation;
			StateChanged(this, new StateChangedEventArgs(states.boardedShip, states.steeringShip));

			//LeanTween.value(walkingCameraSize, interactingCameraSize)
		}

		public void steeringShipToBoardedShip()
		{
			StateChanged (this, new StateChangedEventArgs (states.steeringShip, states.boardedShip));

			Assert.IsTrue(state == states.steeringShip);
			newState = states.boardedShip;
		}


		public void jumpingToLandToOnLand()
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
