using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SeaOfGreed{
	public class PlayerDriver : MonoBehaviour {

		internal states state = states.onLand;
		internal states newState = states.noState;

		public float boardShipRange = 10;
		public float dockShipRange = 10;
		public float dockOffset = .5f;
		public float jumpTime = 1;
		public float jumpScale = 3;
		public float minDistanceToGrabWheel = .5f;

		public float walkSpeed = 2;
		public float width = .12f;
		public float height = .12f;

		public LayerMask groundRaycastMask;
		public LayerMask dockRaycastMask;
		public LayerMask boatRaycastMask;

		public GameObject sprite;

		Animator anim;

		internal GameObject shipBorded;
		internal PlayerController controller;


		public delegate void StateChangedEventHandler(PlayerDriver sender, StateChangedEventArgs e);
		public event StateChangedEventHandler StateChanged;


		// Use this for initialization
		void Start () {
			anim = GetComponent<Animator>();
			controller = gameObject.GetComponent<PlayerController> ();
		}
		
		// Update is called once per frame
		void Update () {
			
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

		internal float getDistanceToWheel()
		{
			return Vector3.Distance(transform.position, shipBorded.GetComponent<ShipController>().wheelMarker.transform.position);
		}



		// state transitions
		internal void boardedShipToOnLand(Vector3 closestDock) // this was a test of the animations, didn't work lol
		{

			var fromLocation = transform.position;
			// get the location to jump to by extending the line between fromLocation and closestDock
			var jumpVector = closestDock - fromLocation;
			var toLocation = fromLocation + (jumpVector.normalized * (jumpVector.magnitude + dockOffset));

			var fromRotation = shipBorded.transform.rotation;

			newState = states.jumpingToLand;


			var originalScale = transform.localScale;

            // scale the character so they look like they're jumping
            LeanTween.value(gameObject,
                (time) => {
                    transform.localScale = new Vector3(
                    originalScale.x + myMath.parabolicScaleCalc(time, jumpScale),
                    originalScale.y + myMath.parabolicScaleCalc(time, jumpScale),
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

		internal void onLandToBoardedShip(GameObject ship)
		{
			var fromLocation = transform.position;
			var toLocation = ship.GetComponent<ShipController>().getClosestBoardingPoint(transform.position);

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

		internal void jumpingToShipToBoardedShip(GameObject ship, Vector3 location)
		{
			Assert.IsTrue(state == states.jumpingToShip);
			transform.rotation = ship.transform.rotation;
			newState = states.boardedShip;
			transform.position = location;
			transform.SetParent(ship.transform);
			shipBorded = ship;
		}

		internal void boardedShipToSteeringShip()
		{
			Assert.IsTrue(state == states.boardedShip);
			newState = states.steeringShip;
			transform.position = shipBorded.GetComponent<ShipController>().wheelMarker.transform.position;
			//sprite.transform.localRotation = shipBorded.GetComponent<ShipController>().wheelMarker.transform.rotation;
			StateChanged(this, new StateChangedEventArgs(states.boardedShip, states.steeringShip));

			//LeanTween.value(walkingCameraSize, interactingCameraSize)
		}

		internal void steeringShipToBoardedShip()
		{
			StateChanged (this, new StateChangedEventArgs (states.steeringShip, states.boardedShip));

			Assert.IsTrue(state == states.steeringShip);
			newState = states.boardedShip;
		}


		internal void jumpingToLandToOnLand()
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
