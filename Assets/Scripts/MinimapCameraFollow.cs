using System;
using UnityEngine;

namespace SeaOfGreed
{
	public class MinimapCameraFollow : MonoBehaviour
	{
		public CharacterDriver player;
		public Vector3 offset;

		public void Start(){
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().minimapCameraBlackout = gameObject.GetComponent<CameraBlackout>();
		}

		public void Update() {
			transform.position = player.transform.position + offset;
			if (player.state == states.steeringShip && GameManager.options.game.MinimapRotateWithShip) {
				transform.rotation = player.shipBorded.transform.rotation;
			} else if (player.state != states.steeringShip && GameManager.options.game.MinimapRotateWithPlayer) {
				transform.rotation = player.sprite.transform.rotation;
			} else {
				//Doesnt work
				transform.rotation = Quaternion.identity;
			}
		}
	}
}

