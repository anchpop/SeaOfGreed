using System;
using UnityEngine;

namespace SeaOfGreed
{
	public class MinimapCameraFollow : MonoBehaviour
	{
		public CharacterDriver player;
		public Vector3 offset;
		public static bool RotateWithPlayer;
		public static bool RotateWithShip;

		public void Start(){
			
		}

		public void Update() {
			transform.position = player.transform.position + offset;
			if (player.state == states.steeringShip && RotateWithShip) {
				transform.rotation = player.shipBorded.transform.rotation;
			}else if (player.state != states.steeringShip && RotateWithPlayer) {
				transform.rotation = player.sprite.transform.rotation;
			} else {
				//Doesnt work
				transform.rotation = Quaternion.identity;
			}
		}
	}
}

